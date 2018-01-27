using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The event to be executed by the input module
/// </summary>
public static class HauoliEvents
{
    /// <summary>
    /// Use the implementing object's ReadBufferData
    /// </summary>
    /// <param name="handler">The implementing object</param>
    /// <param name="eventData">The data being sent to the object</param>
    private static void Execute(IHauoliHandler handler, BaseEventData eventData)
    {
        handler.ReadBufferData(ExecuteEvents.ValidateEventData<HauoliEventData>(eventData));
    }

    /// <summary>
    /// Getter for Execute
    /// </summary>
    public static ExecuteEvents.EventFunction<IHauoliHandler> hauoliEventHandler
    {
        get { return Execute; }
    }
}