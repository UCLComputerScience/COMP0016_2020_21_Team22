using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilteredPlanesCanvas : MonoBehaviour
{
    [SerializeField] private Toggle verticalPlane;
    [SerializeField] private Toggle horizontalPlane;
    [SerializeField] private Toggle bigPlane;
    [SerializeField] private Button startButton;

    private ARfilteredplanes arFilteredPlanes;

    public bool VerticalPlaneToggle 
    { 
        get => verticalPlane.isOn;
        set
        {
            verticalPlane.isOn = value;
            CheckAll();
        }

    }
    public bool HorizontalPlaneToggle
    {
        get => horizontalPlane.isOn;
        set
        {
            horizontalPlane.isOn = value;
            CheckAll();
        }

    }
    public bool BigPlaneToggle
    {
        get => bigPlane.isOn;
        set
        {
            bigPlane.isOn = value;
            CheckAll();
        }

    }

    private void OnEnable()
    {
        arFilteredPlanes = FindObjectOfType<ARfilteredplanes>();

        arFilteredPlanes.OnVerticalPlaneFound += () => VerticalPlaneToggle = true;
        arFilteredPlanes.OnHorizontalPlaneFound += () => HorizontalPlaneToggle = true;
        arFilteredPlanes.OnBigPlaneFound += () => BigPlaneToggle = true;

    }

    private void OnDisable()
    {
        arFilteredPlanes.OnVerticalPlaneFound -= () => VerticalPlaneToggle = true;
        arFilteredPlanes.OnHorizontalPlaneFound -= () => HorizontalPlaneToggle = true;
        arFilteredPlanes.OnBigPlaneFound -= () => BigPlaneToggle = true;
    }
    
    private void CheckAll()
    {
        if (VerticalPlaneToggle && HorizontalPlaneToggle && BigPlaneToggle)
        {
            startButton.interactable = true;
        }
    }
}
