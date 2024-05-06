using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomNotify : UnityEvent<EventArgs> { };

public class EventListener
{
    public CustomNotify OnNotify = new CustomNotify();
}