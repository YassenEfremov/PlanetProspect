using System.Collections;
using System.Collections.Generic;
using System;  // For DateTime
using UnityEngine;

public class Universe : MonoBehaviour {

    /*
     * Planet coordinates are calculated in AU
     * To give a realistic perception of scale in the universe, these values are multiplied
     * this is the coefficient they are multiplied with
     */
    public float distanceScale;

    /*
     * How much time passes per second in the simulation
     * TODO: slow-down / fast-forward time
     */
    public float dayTimeStep;
    public float hourTimeStep;
    public float minuteTimeStep;

    public DateTime georgianDate;
    public double julianDate;
    public double julianCenturiesSinceEpoch;
    readonly static int julianCenturyDays = 36525; // In Julian days
    readonly static double julianEpoch = 2451545.0;  // In Julian days

    /* 
     * (Gravitational Constant * Earth's Mass) / (distance from sun to earth * distance scale)
     * This variable is precomputed for use in all gravitational computations and contains all the constant values required
     * Aside from the gravitational constant, all other values unify the different parameters:
     * distance from sun to earth in km && distance scale - unifies AU
     * Earth's mass -> All planets mass, which are otherwise too large to store in a 64 bit integer
     * Constant values approximations:
     * Earth's mass -> 5.9722 * 10^24 kg
     * Distance from earth to moon -> 150 * 10^10 m
     * Gravitational constant -> 6.67 * 10^-11 Nm^2/kg^2
     * TODO: More appropriate and less misleading var name
     */
    public static float gravitationalConstant = 265.563826f;
    public readonly static float physicsTimeStep = 0.01f;

    void Awake() {
        georgianDate = DateTime.Now;
        gravitationalConstant /= distanceScale;
        print(gravitationalConstant);

        hourTimeStep += dayTimeStep * 24;
        minuteTimeStep += hourTimeStep * 60;
    }

    void FixedUpdate() {
        georgianDate = georgianDate.AddMinutes(minuteTimeStep * Time.fixedDeltaTime);

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
