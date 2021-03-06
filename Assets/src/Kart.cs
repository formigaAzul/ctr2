﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kart
{

    public CameraController cm1c;
    private KartController kc;
    public KartScript kart_script;

    private GUIText pointText;
    public GUIText guitextApples;

    public GameObject guiArme;
    public GameObject camera;
    public GameObject c2d;
    public GameObject superLight;
    public GameObject superLightWeapon;
    public GameObject guiPoints, plus1;

    public int lastWeaponTextureNb = -1;
    public int numeroJoueur;
    public int nbPoints = 0;
    public int nbApples = 0;
    public int nbApplesFinal = 0;
    public bool isWinner = false;

    public static int nPlayer = 0;
    public static int totalPlayers = 1;
    private static float speedCoeff;
    private static float turnCoeff;

    public Kart(Vector3 pos, Quaternion q, string kart)
    {
        nbPoints = 0;
        numeroJoueur = ++nPlayer;
        InitObjet(pos, q, kart);
    }

    public static void setCoefficients(float speed, float turn)
    {
        speedCoeff = speed;
        turnCoeff = turn;
    }

    public void InitObjet(Vector3 pos, Quaternion q, string kartName)
    {
        GameObject kart = GameObject.Instantiate(Resources.Load(kartName), pos, q) as GameObject;
        //GameObject kart_angles = GameObject.Instantiate (Resources.Load("GameplayObject"), pos, q) as GameObject;
        kart.name = kart.name.Split('(')[0];
        kc = kart.GetComponent<KartController>();
        kart_script = kart.GetComponent<KartScript>();
        
        kart_script.SetKart(this);

        kc.setCoefficients(speedCoeff, turnCoeff);
        //kart_angles.GetComponent<Gameplay> ().SetKart (kart.transform);
    }

    public void blackScreen()
    {
        camera.GetComponent<Camera>().enabled = false;
        c2d.GetComponent<Camera>().enabled = false;
    }

    public void normalScreen()
    {
        camera.GetComponent<Camera>().enabled = true;
        c2d.GetComponent<Camera>().enabled = true;
    }

    public void SetIllumination(bool a)
    {
        if (a)
        {
            superLight.GetComponent<Light>().color = new Color(114, 113, 0);
            superLightWeapon.GetComponent<Light>().color = new Color(114, 113, 0);
        }
        else
        {
            superLight.GetComponent<Light>().color = new Color();
            superLightWeapon.GetComponent<Light>().color = new Color();
        }
    }
        public void AddPoint(int n)
    {
        // n = 1 or n = -1
        if (KartController.IA_enabled)
            return;
        nbPoints += n;
        if (pointText)
            pointText.text = nbPoints.ToString();
        if (nbPoints == Game.Instance.MaxScore)
        {
            isWinner = true;
            kc.gameObject.AddComponent<Party>();
            KartController.IA_enabled = true;
            Main.statistics.endGame();
        }
    }

    public void addApples()
    {
        int n = Random.Range(4, 8);
        nbApplesFinal = System.Math.Min(10, nbApplesFinal + n);
        kart_script.animApples();
    }

    public void rmApples(int n)
    {
        nbApplesFinal -= n;
        nbApples -= n;
        nbApplesFinal = System.Math.Max(0, nbApplesFinal);
        nbApples = System.Math.Max(0, nbApples);
        if (nbApples != 10) SetIllumination(false);
        if (guitextApples)
            guitextApples.text = "x " + nbApples.ToString();
    }

    public bool IsSuper()
    {
        return nbApples == 10;
    }

}










