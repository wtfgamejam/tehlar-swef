using UnityEngine;
using System.Collections;
using Tim;

public class TehlarSwef : MonoBehaviour
{

    public int pixelsPerUnit = 10;
    float onePixel;
    public float moveSpeed = 0.01f;
    Transform trans;
    BoxCollider2D c2d;
    Bounds bounds;
    float trueX;
    float trueY;
    Vector3 pos;
    float ax;
    float axDir;
    float ay;
    float ayDir;
    float dist;
    Vector2[] raysSource = new Vector2[3];
    RaycastHit2D[] rays = new RaycastHit2D[3];
    float[] rayDist = new float[3];

    // Use this for initialization
    void Start()
    {
        trans = this.GetComponent<Transform>();
        c2d = this.GetComponent<BoxCollider2D>();
        onePixel = 1 / pixelsPerUnit;
        trueX = 0.0f;
        trueY = 0.0f;
    }

    void Update()
    {
        pos = trans.position;
        bounds = c2d.bounds;

        ax = Input.GetAxis("Horizontal");
        axDir = Mathf.Sign(ax);
        ay = Input.GetAxis("Vertical");
        ayDir = Mathf.Sign(ay);

        dist = (moveSpeed / pixelsPerUnit) * Time.deltaTime;

        if (ax != 0.0f)
        {
            trueX += axDir * Mathf.Min(dist, Mathf.Abs(testDistance(bounds, new Vector2(axDir, 0.0f), dist)));
        }

        if (ay != 0.0f)
        {
            trueY += ayDir * Mathf.Min(dist, Mathf.Abs(testDistance(bounds, new Vector2(0.0f, ayDir), dist)));
        }
        
        if(Mathf.Abs(trueX) + Mathf.Abs(trueY) >= onePixel)
        {
            trueX += pos.x;
            trueY += pos.y;
    
            pos.x = UtilityMethods.RoundTo(trueX, pixelsPerUnit / 10);
            pos.y = UtilityMethods.RoundTo(trueY, pixelsPerUnit / 10);
    
            trueX -= pos.x;
            trueY -= pos.y;
    
            trans.position = pos;
        }
    }

    float testDistance(Bounds aabb, Vector2 direction, float distance)
    {
        Vector3 min = aabb.min;
        Vector3 max = aabb.max;
        Vector3 cen = aabb.center;

        if (direction.x == -1.0f)
        {
            //Left
            raysSource [0] = new Vector2(min.x - 0.01f, max.y - 0.01f);
            raysSource [1] = new Vector2(min.x - 0.01f, cen.y);
            raysSource [2] = new Vector2(min.x - 0.01f, min.y + 0.01f);
        }
        else if (direction.x == 1.0f)
        {
            //Right
            raysSource [0] = new Vector2(max.x + 0.01f, max.y - 0.01f);
            raysSource [1] = new Vector2(max.x + 0.01f, cen.y);
            raysSource [2] = new Vector2(max.x + 0.01f, min.y + 0.01f);
        }

        if (direction.y == 1.0f)
        {
            //Up
            raysSource [0] = new Vector2(min.x + 0.01f, max.y + 0.01f);
            raysSource [1] = new Vector2(cen.x, max.y + 0.01f);
            raysSource [2] = new Vector2(max.x - 0.01f, max.y + 0.01f);
        }
        else if (direction.y == -1.0f)
        {
            //Down
            raysSource [0] = new Vector2(min.x + 0.01f, min.y - 0.01f);
            raysSource [1] = new Vector2(cen.x, min.y - 0.01f);
            raysSource [2] = new Vector2(max.x - 0.01f, min.y - 0.01f);
        }

        for (var i = 0; i < 3; i++)
        {
            rays [i] = Physics2D.Raycast(raysSource [i], direction);
            if (rays [i].collider != null)
            {
                rayDist [i] = rays [i].distance;
            }
            else
            {
                rayDist [i] = distance;
            }
            //Debug.DrawRay(raysSource [i], direction, Color.red);
        }
        return Mathf.Min(rayDist [0], Mathf.Min(rayDist [1], rayDist [2]));
    }
}
