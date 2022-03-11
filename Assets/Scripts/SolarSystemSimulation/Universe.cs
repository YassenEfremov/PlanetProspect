using System.Collections;
using System.Collections.Generic;
using System;  // For DateTime
using UnityEngine;

public class Universe : MonoBehaviour {

    public DateTime georgianDate;

    void Awake() {
        georgianDate = DateTime.Now;
        print(georgianDate);
    }

    void FixedUpdate() {
        
    }
}
