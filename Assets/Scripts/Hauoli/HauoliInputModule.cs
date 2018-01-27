using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The input module which uses HauoliTracker for coordinate information
/// </summary>
public class HauoliInputModule : BaseInputModule
{
    // Constant for GameObjects associated with this input module
    private const string trackableTag = "HauoliTrackable";

    // Instance variables
    public GameObject[] targetObjects;
    private HauoliTracker ht;

    /// <summary>
    /// Return the HauoliTracker
    /// </summary>
    public HauoliTracker GetHauoliTracker(){
        return ht;
    }

    /// <summary>
    /// Return the TrackableTag
    /// </summary>
    public string GetTrackableTag()
    {
        return trackableTag;
    }

    /// <summary>
    /// This module starts up a <see cref="HauoliTracker"/> to get coordinate information
    /// and sends the data to objects with "trackableTag"
    /// </summary>
    public override void ActivateModule()
    {
        base.ActivateModule();
        ht = new HauoliTracker();
        targetObjects = GameObject.FindGameObjectsWithTag(trackableTag);
    }

    /// <summary>
    /// Stop the <see cref="HauoliTracker"/> thread
    /// </summary>
    public override void DeactivateModule()
    {
        base.DeactivateModule();
        if (ht != null)
            ht.Interrupt();
    }

    /// <summary>
    /// Sends the event data for every tick from the <see cref="HauoliTracker"/>
    /// </summary>
    public override void Process()
    {
        // Do not process when there are no targetObjects
        if (targetObjects == null || targetObjects.Length == 0)
            return;

        // Send buffer data and mouse
        if (ht != null)
        {
            foreach (var target in targetObjects)
                ExecuteEvents.Execute(target, new HauoliEventData(eventSystem, ht.mCoor, ht.mouseClick, ht.mGyro), HauoliEvents.hauoliEventHandler);
        }

    }
}