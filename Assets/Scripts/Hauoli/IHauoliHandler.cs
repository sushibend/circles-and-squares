using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Interface to be implemented by GameObjects that use this input module
/// Based on https://gist.github.com/stramit/76e53efd67a2e1cf3d2f
/// </summary>
public interface IHauoliHandler : IEventSystemHandler
{
    /// <summary>
    /// Handles the decoding for the eventData inside of a GameObject
    /// </summary>
    /// <param name="eventData">The event data sent from the input module to be decoded</param>
    void ReadBufferData(HauoliEventData eventData);
}