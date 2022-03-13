using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

public class TestToJulianDate
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestToJulianDateSimplePasses()
    {
        var dateString = "5/1/2008 8:30:52 AM";
        DateTime date1 = DateTime.Parse(dateString,
                                  System.Globalization.CultureInfo.InvariantCulture);
        Assert.AreEqual(2454587.85476852, Universe.ToJulianDate(date1));

    }

}
