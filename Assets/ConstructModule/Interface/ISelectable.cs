using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public interface ISelectable {
    Transform TransformComponent { get; }
}
