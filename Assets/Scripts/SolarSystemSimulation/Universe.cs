using System.Collections;
using System.Collections.Generic;
using System;  // For DateTime
using UnityEngine;

public class Universe : MonoBehaviour {

    public float dayTimeStep;
    public DateTime georgianDate;
    public double julianDate;
    public double julianCenturiesSinceEpoch;

    readonly static int julianCenturyDays = 36525; // In Julian days
    readonly static double julianEpoch = 2451545.0;  // In Julian days

    void Awake() {
        georgianDate = DateTime.Now;
    }

    void FixedUpdate() {
        georgianDate = georgianDate.AddDays(dayTimeStep * Time.fixedDeltaTime);
        julianDate = ToJulianDate(georgianDate);
        julianCenturiesSinceEpoch = ToJulianCenturiesSinceEpoch(julianDate);
        print("Georgian date: " + georgianDate);
    }

    /*
     * ToOADate is similar to Julian Dates except it uses a different starting point (December 30, 1899)
     * The Julian Date to December 30th 1899 midnight is 2415018.5
     */
    public static double ToJulianDate(DateTime georgianDate) {
        return georgianDate.ToOADate() + 2415018.5;
    }

    public static double ToJulianCenturiesSinceEpoch(double julianDate) {
        return (julianDate - julianEpoch) / julianCenturyDays;
    }

    


}
