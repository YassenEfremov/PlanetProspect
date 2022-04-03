using System.Collections;
using System.Collections.Generic;
using System;  // For DateTime
using UnityEngine;

public class Universe : MonoBehaviour {

    public float distanceScale = 30;

    public float dayTimeStep;
    public DateTime georgianDate;
    public double julianDate;
    public double julianCenturiesSinceEpoch;

    readonly static int julianCenturyDays = 36525; // In Julian days
    readonly static double julianEpoch = 2451545.0;  // In Julian days

    // Gravity is used when simulating rockets (TODO: satelites and moons?)
    // public readonly static long earthMass = 60000000000000000000;
    // public readonly static double gravitationalConstant = 0.0000000000673;

    // (Gravitational Constant * Earth's Mass) / (distance from sun to earth * distanceScale)
    public static float gravitationalConstant;

    public readonly static float physicsTimeStep = 0.01f;

    void Awake() {
        georgianDate = DateTime.Now;
        gravitationalConstant = 26.692f / distanceScale;
        print(gravitationalConstant);
    }

    void FixedUpdate() {
        georgianDate = georgianDate.AddDays(dayTimeStep * Time.fixedDeltaTime);
        julianDate = ToJulianDate(georgianDate);
        julianCenturiesSinceEpoch = ToJulianCenturiesSinceEpoch(julianDate);
        //print("Georgian date: " + georgianDate);
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
