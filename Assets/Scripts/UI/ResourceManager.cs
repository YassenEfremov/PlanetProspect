using UnityEngine;
using UnityEngine.UI;


public class ResourceManager : MonoBehaviour
{
    public int energy { get; private set; } = 6000;
    public int steel { get; private set; } = 1000;

    public int maxEnergy { get; private set; } = 10000;
    public int MaxEnergy
    {
        get => maxEnergy;
        set
        {
            if (value > 0)
                maxEnergy = value;
        }
    }
    public int maxSteel { get; private set; } = 10000;
    public int MaxSteel
    {
        get => maxSteel;
        set
        {
            if (value > 0)
                maxSteel = value;
        }
    }

    [SerializeField] Slider energyBar;
    [SerializeField] Slider steelBar;


    void Start()
    {
        energyBar.maxValue = maxEnergy;
        steelBar.maxValue = maxSteel;
        energyBar.value = energy;
        steelBar.value = steel;
        energyBar.GetComponentInChildren<Text>().text = energy.ToString();
        steelBar.GetComponentInChildren<Text>().text = steel.ToString();
    }


    public void addEnergy(int amount)
    {
        if (energy + amount <= maxEnergy)
        {
            energy += amount;
            energyBar.value = energy;
            energyBar.GetComponentInChildren<Text>().text = energy.ToString();
        }
    }

    public void addSteel(int amount)
    {
        if (steel + amount <= maxSteel)
        {
            steel += amount;
            steelBar.value = steel;
            steelBar.GetComponentInChildren<Text>().text = steel.ToString();
        }
    }

    public void subtractEnergy(int amount)
    {
        if (energy - amount >= 0)
        {
            energy -= amount;
            energyBar.value = energy;
            energyBar.GetComponentInChildren<Text>().text = energy.ToString();
        }
    }

    public void subtractSteel(int amount)
    {
        if (steel - amount >= 0)
        {
            steel -= amount;
            steelBar.value = steel;
            steelBar.GetComponentInChildren<Text>().text = steel.ToString();
        }
    }
}
