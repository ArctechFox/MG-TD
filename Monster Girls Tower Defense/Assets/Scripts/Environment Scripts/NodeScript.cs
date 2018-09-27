using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    public Color HoverColorOccupied;
    public Color HoverColorEmpty;

    private Color _invisible = new Color(1.0f, 1.0f, 1.0f, 0f);
    private MeshRenderer _rend;

    private GameObject _occupant;
    private bool _isMouseDown = false;

    //private BuildManager _buildManager;

    void Start()
    {
        _rend = GetComponent<MeshRenderer>();
        _rend.material.color = _invisible;
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0)) PlaceUnit();
        //if (Input.GetMouseButtonDown(1)) SellUnit();
    }

    private void OnMouseOver()
    {
        _rend.enabled = true;
        _rend.material.color = _occupant != null ? HoverColorOccupied : HoverColorEmpty;
        Debug.Log(string.Format("OnMouseEnter triggered on object {0}", gameObject.name));
    }

    private void OnMouseExit()
    {
        _rend.material.color = _invisible;
    }

    private void OnMouseDown()
    {
        PlaceUnit();
    }

    private void PlaceUnit()
    {
        GameObject buildablePrefab = BuildManager.Instance.CurrentBuildableObject;
        var buildableInstance = Instantiate(buildablePrefab, transform.position, transform.rotation);

        try
        {
            buildableInstance.SendMessage("InitializeComponents", gameObject.transform);
            _occupant = buildableInstance;

            Debug.Log(String.Format("Placed object {0} on node {1}", _occupant.name, gameObject.name));
        }
        catch (Exception ex)
        {
            Debug.LogError("Must place a Buildable object. (Couldn't find script 'Buildable' on instance)");
            Destroy(buildableInstance);
        }
    }

    private void SellUnit()
    {
        // TODO: Revise and expand this method... a lot
        if (_occupant != null)
        {
            Destroy(_occupant);
        }
    }
}
