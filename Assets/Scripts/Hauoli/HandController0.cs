using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HandController0 : MonoBehaviour, IHauoliHandler
{
    private int frame = 0;

    // an clientIndex corresponds to a phone
    private int clientIndex = 0;

    // Screen Config
    private float[][] RANGES_RCV;
    private float[][] RANGES_DISP;
    private float[] HAUOLI_CENTER = new float[3];
    private float[] SCREEN_CENTER = new float[3];
    private const int maxDim = 3;
    private float[] METER_TO_PIXEL_SCALE;

    private enum Mode { SinglePlayer, TwoPlayer };
    private Mode mode;

    // The following light
    public Light followingLight;

    // Received Information
    private float[] mCoor = new float[maxDim];
    private float[] mGyro = new float[maxDim];
    private bool mouse = false;

    // Movement sensitivity
    public GameObject eventSystem;
    private HauoliInputModule hauoliInputModeule;
    private HauoliTracker hauoliTracker = null;
    private float sensitivityHauoliX;
    private float sensitivityHauoliY;
    private float sensitivityHauoliZ;

    // For debug
    private float lastTimePrintHauoliInfo = 0;
    private bool isDebugHauoliInfo = false;
    private const float printHauoliInfoSpan = 0.8f;

    private float lastTimePrintPosInfo = 0;
    private bool isDebugPosInfo = true;
    private const float printPosInfoSpan = 0.8f;

    private bool isDebugResetPos = true;

    // Use this for initialization
    void Start () {
        Debug.Log(String.Format("HandController 0 Start: {0}", clientIndex));

        hauoliInputModeule = eventSystem.GetComponent<HauoliInputModule>();

        SetStaticParameters();
    }

    void SetStaticParameters()
    {

        // XYZ are defined as on phone, which is different from that in Unity (Y<->Z)
        RANGES_RCV = new float[maxDim][];

        // X-axis in HauoliTracking Coordinate System
        RANGES_RCV[0] = new float[2];
        RANGES_RCV[0][0] = -0.1f;
        RANGES_RCV[0][1] = 1f;
        // Y-axis in HauoliTracking Coordinate System
        RANGES_RCV[1] = new float[2];
        RANGES_RCV[1][0] = -0.86f;
        RANGES_RCV[1][1] = -0.16f;
        // Z-axis in HauoliTracking Coordinate System
        RANGES_RCV[2] = new float[2];
        RANGES_RCV[2][0] = 0f;
        RANGES_RCV[2][1] = 0.7f;

        mCoor[0] = -2f;
        mCoor[1] = 7f;
        mCoor[2] = -17f;
        // -------------------------

        RANGES_DISP = new float[maxDim][];
	
        // X-axis in Unity Coordinate System = X-axis in HauoliTracking
        RANGES_DISP[0] = new float[2];
        RANGES_DISP[0][0] = -13.5f;
        RANGES_DISP[0][1] = 9.5f;
        // Z-axis in Unity Coordinate System = Y-axis in 2D HauoliTracking
        RANGES_DISP[1] = new float[2];
        RANGES_DISP[1][0] = -18f;
        RANGES_DISP[1][1] = -8f;
        // Y-axis in Unity Coordinate System = Z-axis in 2D HauoliTracking
        RANGES_DISP[2] = new float[2];
        RANGES_DISP[2][0] = -10f;
        RANGES_DISP[2][1] = 10f;
        // -------------------------

        METER_TO_PIXEL_SCALE = new float[maxDim];
        for (int i = 0; i < maxDim; i++)
        {
            METER_TO_PIXEL_SCALE[i] = (float)((RANGES_DISP[i][1] - RANGES_DISP[i][0]) / (RANGES_RCV[i][1] - RANGES_RCV[i][0]));
        }
        Debug.Log(String.Format("Controller 0 - Meter to Pixel Scale: {0}, {1}, {2}"
                                , METER_TO_PIXEL_SCALE[0], METER_TO_PIXEL_SCALE[1], METER_TO_PIXEL_SCALE[2]));

        // Initialize sensitivity
        sensitivityHauoliX = 5.6f;
        sensitivityHauoliY = 2.6f;
        sensitivityHauoliZ = 4.6f;

        SetToSinglePlayer();
    }

    void SetToSinglePlayer(){
        Debug.Log("Controller 0 set to single-player mode");
        mode = Mode.SinglePlayer;

        // Initial position of the phone, in the order of x,y,z in Hauoli Coordinate System
        HAUOLI_CENTER[0] = 0.5f;
        HAUOLI_CENTER[1] = -1f;
        HAUOLI_CENTER[2] = 0.2f;

        // Initial position of the phone in the screen, in the order of x,y,z in Unity System
        SCREEN_CENTER[0] = -2f;
        SCREEN_CENTER[1] = 7f;
        SCREEN_CENTER[2] = -17f;
    }

    void SetToTwoPlayers(){
        Debug.Log("Controller 0 set to two-player mode");
        mode = Mode.TwoPlayer;

        // Initial position of the phone, in the order of x,y,z in Hauoli Coordinate System
        HAUOLI_CENTER[0] = 0.3f;
        HAUOLI_CENTER[1] = -0.1f;
        HAUOLI_CENTER[2] = 0.2f;

        // Initial position of the phone in the screen, in the order of x,y,z in Unity System
        SCREEN_CENTER[0] = -7f;
        SCREEN_CENTER[1] = 7f;
        SCREEN_CENTER[2] = -17f;
    }

    void SetHauoliCenterToCurrentPos(){
        if(isDebugResetPos){
            Debug.Log("Reset Pos to " + mCoor[0] + ", " 
                      + mCoor[1] + ", " + mCoor[2]);
        }

        for (int i = 0; i < 3; i++){
            HAUOLI_CENTER[i] = mCoor[i];
        }
    }

    float[] ConvertCoor(float[] coor)
    {
        float[] ret = new float[maxDim];
        for (int di = 0; di < maxDim; di++)
        {
            switch (di)
            {
                case 0:
                    ret[di] = (coor[di] - HAUOLI_CENTER[0])
                        * METER_TO_PIXEL_SCALE[di] * sensitivityHauoliX + SCREEN_CENTER[0];
                    break;
                case 1:
                    ret[di] = (coor[di] - HAUOLI_CENTER[1])
                        * METER_TO_PIXEL_SCALE[di] * sensitivityHauoliY + SCREEN_CENTER[2];
                    break;
                case 2:
                    ret[di] = (coor[di] - HAUOLI_CENTER[2])
                        * METER_TO_PIXEL_SCALE[di] * sensitivityHauoliZ + SCREEN_CENTER[1];
                    break;
                default:
                    break;
            }
        }

        return ret;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Determine it is in Single Player Mode or Multiple Player Mode
        if(hauoliTracker != null){
            if(hauoliTracker.GetNumberOfConnection() <= 1 && !(mode.Equals(Mode.SinglePlayer))){
                SetToSinglePlayer();
            }else if(hauoliTracker.GetNumberOfConnection() > 1 && !(mode.Equals(Mode.TwoPlayer))){
                SetToTwoPlayers();
            }
        }

        // if mouse is down, reset the origin point to current position
        if (mouse)
        {
            SetHauoliCenterToCurrentPos();
        }

        float[] ret = ConvertCoor(mCoor);

        if (isDebugPosInfo)
        {
            float currentTime = Time.time;
            if (currentTime - lastTimePrintPosInfo >= printPosInfoSpan)
            {
                string strDebug = "before = " + mCoor[0] + "," + mCoor[1] + "," + mCoor[2]
                    + "\n" + "after = " + ret[0] + "," + ret[1] + "," + ret[2];
                Debug.Log(strDebug);

                lastTimePrintPosInfo = currentTime;
            }
        }

        transform.SetPositionAndRotation(
            new Vector3(ret[0], ret[2], ret[1]),
            new Quaternion(0, 0, 0, 0));

        followingLight.transform.SetPositionAndRotation(
            new Vector3(ret[0], 16f, ret[1]),
            Quaternion.Euler(90f, 0f, 0f));
        frame++;
    }


    public void ReadBufferData(HauoliEventData eventData)
    {
        // Initialize the hauoli tracker
        if(hauoliTracker == null){
            hauoliTracker = hauoliInputModeule.GetHauoliTracker();
        }

        for (int i = 0; i < maxDim; i++)
        {
            mCoor[i] = eventData.mCoor[clientIndex][i];
        }
        mouse = eventData.mouse[clientIndex];

        if (isDebugHauoliInfo)
        {
            float currentTime = Time.time;
            if (currentTime - lastTimePrintHauoliInfo >= printHauoliInfoSpan)
            {
                Debug.Log("hand 0 rcv = " + mCoor[0] + "," + mCoor[1] + "," + mCoor[2]);
                lastTimePrintHauoliInfo = currentTime;
            }
        }
    }
}
