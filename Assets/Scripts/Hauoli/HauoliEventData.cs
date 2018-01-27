using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// HauoliEventData contains the buffer data from the EventSystem
/// </summary>
public class HauoliEventData : BaseEventData
{
    //  Instance variables
    public float[][] mCoor = new float[HauoliTracker.maxConn][];
    public float[][] mGyro = new float[HauoliTracker.maxConn][];
    public bool[] mouse = new bool[HauoliTracker.maxConn];

    /// <summary>
    /// <see cref="HauoliEventData"> constructor with the buffer to transmit to the GameObject
    /// </summary>
    /// <param name="eventSystem">The event system that produced this event</param>
    /// <param name="x">The x coordinate</param>
    /// <param name="y">The y coordinate</param>
    /// <param name="z">The z coordinate</param>
    /// <param name="m">The mouse flags</param>
    public HauoliEventData(EventSystem eventSystem, float[][] coor, bool[] m, float[][] gyro) : base(eventSystem)
    {
        this.mCoor = coor;
        this.mGyro = gyro;
        this.mouse = m;
    }
}