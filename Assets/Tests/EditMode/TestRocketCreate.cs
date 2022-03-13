using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DirectionTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void assertRocketIsMade()
    {
        GravityObject gravityObject = new GravityObject();
        gravityObject.isGravityAffected = true;

        Assert.AreEqual(true, gravityObject.isGravityAffected);
    }

}
