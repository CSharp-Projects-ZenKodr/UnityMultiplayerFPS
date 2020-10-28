﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pistol : Weapon
{
    public GameObject refer;
    Transform CamHandlerObject;
    //public bool canhurtplayer = true;

    public Pistol() {
        id = 0;
        name = "Pistol";
        automatic = false;
        fire_rate = 0.1f;
        //maxAmmo = 50;
        TransferVelocity = true;
        TimeToPressTrigger = 0.0f;
        slot = 1;
        //lifeDuration = 30.0f;
        //speed = 10.0f;
        //bulletPrefab = (GameObject)Resources.Load("bullet.prefab");
    }

    void Start()
    {
        bulletPrefab = refer;
       //CamHandlerObject = gameObject.GetComponentInParent<Transform>();
        CamHandlerObject = transform.parent.parent;
    }

    override public void Shoot() {
        //Ray ScreenVector = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f));

        int layer = ~(1 << 9); // маска слоя все кроме физического коллайдера персонажей и триггер-коллайдеров самого игрока
        //if (!canhurtplayer) layer &= ~(1<<12); // и триггер-коллайдеров самого игрока
        Ray ScreenVector = new Ray(CamHandlerObject.position, CamHandlerObject.forward);

        //RaycastHit hit;
        //bool h = Physics.Raycast(ScreenVector,out hit, Mathf.Infinity ,layer);
        //GameObject flare = Instantiate(bulletPrefab, BarrelEnd.position, BarrelEnd.rotation);
        //if (h) /*GameObject shot = */Instantiate(bulletPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        //Debug.Log("Shot with Pistol");

        //Debug.Log(hit.collider);
        //if (h && (hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 12)) {
        //    hit.collider.transform.root.SendMessage("DoDamage", 1.0f, SendMessageOptions.DontRequireReceiver); // потому что health ставлю на родителя, а хитбоксы - на детях.
        //}

        //----------------------------------------------
        // другой способ

        RaycastHit[] hits = Physics.RaycastAll(ScreenVector, Mathf.Infinity, layer).OrderBy(h => h.distance).ToArray();
        GameObject flare = Instantiate(bulletPrefab, BarrelEnd.position, BarrelEnd.rotation);
        Debug.Log("Shot with Pistol");
        if (hits != null)
        {
            for (int i = 0; i < hits.Length; ++i)
            {
                Debug.Log(hits[i].collider);
                if (hits[i].collider.transform.root.gameObject != this.owner) {
                    Instantiate(bulletPrefab, hits[i].point, Quaternion.LookRotation(hits[i].normal));
                    if (hits[i].collider.gameObject.layer == 10)
                    {
                        //hits[i].collider.transform.root.SendMessage("DoDamage", 1.0f, SendMessageOptions.DontRequireReceiver);
                        hits[i].collider.transform.root.SendMessage("DoDamage", new object[2] {1.0f, this.owner}, SendMessageOptions.DontRequireReceiver);
                    }
                    break;
                }

            }
        }

    }
}
