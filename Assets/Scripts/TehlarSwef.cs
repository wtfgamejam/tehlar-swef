﻿using UnityEngine;
using System.Collections;
using System.Linq;
using Tim;

public class TehlarSwef : MonoBehaviour
{

    public int pixelsPerUnit = 10;
    public float moveSpeed = 0.01f;
    public Sprite humanSprite;
    public Sprite alienSprite;
    public bool alienForm = false;
    public float health = 1.0f;

    public AudioClip transformSound;

    string healthChar = "█";

    Transform trans;
    SpriteRenderer sprite;
    BoxCollider2D c2d;
    Bounds bounds;
    AudioSource audio;
    float onePixel;
    float trueX;
    float trueY;

    Vector3 pos;
    float ax;
    float axDir;
    float ay;
    float ayDir;
    float dist;

    Vector2[] raysSource = new Vector2[5];
    RaycastHit2D[] rays = new RaycastHit2D[5];
    float[] rayDist = new float[5];

    // Use this for initialization
    void Start()
    {
        trans = this.GetComponent<Transform>();
        sprite = this.GetComponent<SpriteRenderer>();
        sprite.sprite = alienForm ? alienSprite : humanSprite;
        c2d = this.GetComponent<BoxCollider2D>();
        audio = this.GetComponent<AudioSource>();
        onePixel = 1.0f / pixelsPerUnit;
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

        if (Input.GetButtonDown("Fire1"))
        {
            alienForm = !alienForm;
            if (alienForm)
            {
                audio.clip = transformSound;
                audio.Play();
                sprite.sprite = alienSprite;
            }
            else
            {
                sprite.sprite = humanSprite;
            }
        }

        if (ax != 0.0f)
        {
            trans.rotation = axDir > 0.0f ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
            trueX += axDir * Mathf.Min(dist, Mathf.Abs(testDistance(bounds, new Vector2(axDir, 0.0f), dist)));
        }

        if (ay != 0.0f)
        {
            trueY += ayDir * Mathf.Min(dist, Mathf.Abs(testDistance(bounds, new Vector2(0.0f, ayDir), dist)));
        }
        
        if (Mathf.Abs(trueX) + Mathf.Abs(trueY) >= onePixel)
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
        Vector3 ext = aabb.extents;

        if (direction.x == -1.0f)
        {
            //Left
            raysSource[0] = new Vector2(min.x - 0.01f, max.y - 0.01f);
            raysSource[1] = new Vector2(min.x - 0.01f, max.y - (ext.y / 2.0f));
            raysSource[2] = new Vector2(min.x - 0.01f, cen.y);
            raysSource[3] = new Vector2(min.x - 0.01f, min.y + (ext.y / 2.0f));
            raysSource[4] = new Vector2(min.x - 0.01f, min.y + 0.01f);
        }
        else if (direction.x == 1.0f)
        {
            //Right
            raysSource[0] = new Vector2(max.x + 0.01f, max.y - 0.01f);
            raysSource[1] = new Vector2(max.x + 0.01f, max.y - (ext.y / 2.0f));
            raysSource[2] = new Vector2(max.x + 0.01f, cen.y);
            raysSource[3] = new Vector2(max.x + 0.01f, min.y + (ext.y / 2.0f));
            raysSource[4] = new Vector2(max.x + 0.01f, min.y + 0.01f);
        }

        if (direction.y == 1.0f)
        {
            //Up
            raysSource[0] = new Vector2(min.x + 0.01f, max.y + 0.01f);
            raysSource[1] = new Vector2(max.x - (ext.x / 2.0f), max.y + 0.01f);
            raysSource[2] = new Vector2(cen.x, max.y + 0.01f);
            raysSource[3] = new Vector2(min.x + (ext.x / 2.0f), max.y + 0.01f);
            raysSource[4] = new Vector2(max.x - 0.01f, max.y + 0.01f);
        }
        else if (direction.y == -1.0f)
        {
            //Down
            raysSource[0] = new Vector2(min.x + 0.01f, min.y - 0.01f);
            raysSource[1] = new Vector2(max.x - (ext.x / 2.0f), min.y - 0.01f);
            raysSource[2] = new Vector2(cen.x, min.y - 0.01f);
            raysSource[3] = new Vector2(min.x + (ext.x / 2.0f), min.y - 0.01f);
            raysSource[4] = new Vector2(max.x - 0.01f, min.y - 0.01f);
        }

        for (var i = 0; i < 5; i++)
        {
            rays[i] = Physics2D.Raycast(raysSource[i], direction);
            if (rays[i].collider != null)
            {
                rayDist[i] = rays[i].distance;
            }
            else
            {
                rayDist[i] = distance;
            }
            //Debug.DrawRay(raysSource[i], direction, Color.red);
        }
        return rayDist.Min();
    }
}
