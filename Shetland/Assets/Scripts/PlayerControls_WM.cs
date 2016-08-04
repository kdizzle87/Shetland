﻿using UnityEngine;

public class PlayerControls_WM : MonoBehaviour {

	public NavMeshAgent _agent;
	public Vector3 _startPos;
	public bool _movingToFactory;
	public bool _movingToResource;
	public bool _movingToTown;
	public float _range;
	public Vector3 _objectPos;
	public Factories _factoryScript;
	public ResourceGen _resourceScript;
	public ActivateFactory _activateScript;
	public TownCanvas _townCanvas;

	// Use this for initialization
	void Start () {		
		Spawn();
	}
	
	// Update is called once per frame
	void Update () {
		DetectInput();
		MoveToObject();
	}

	void Spawn(){
		_townCanvas = GameObject.Find("TownCanvas").GetComponent<TownCanvas>();
		_agent = gameObject.GetComponent<NavMeshAgent>();
		_activateScript = gameObject.GetComponent<ActivateFactory>();
		transform.position = _startPos*10;
		_agent.enabled = true;
		_agent.Stop();		
	}

	void DetectInput(){
		if (Input.GetMouseButton(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)){
				if (hit.collider.tag == "Ground"){
					float dist = Vector3.Distance(hit.point, transform.position);
					if (dist > 1.0f){
						_agent.Resume();
						_agent.SetDestination(hit.point);
					}
				}		
			}
		}
		if (Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)){
				switch (hit.collider.tag){
					case "Resource":
					_objectPos = hit.transform.position;
					_agent.SetDestination(_objectPos);
					_agent.Resume();
					_movingToResource= true;
					_resourceScript = hit.collider.gameObject.GetComponent<ResourceGen>();
					_range = 3.0f;
					break;
					case "Factory":
					_objectPos = hit.transform.position;
					_agent.SetDestination(_objectPos);
					_agent.Resume();
					_movingToFactory= true;
					_factoryScript = hit.collider.gameObject.GetComponent<Factories>();
					_range = 5.0f;
					break;
					case "Town":
					_objectPos = hit.transform.FindChild("Entrance").position;
					_agent.SetDestination(_objectPos);
					_agent.Resume();
					_townCanvas._townManager = hit.collider.gameObject.GetComponent<TownManager>();
					_movingToTown = true;
					_range = 1.0f;				
					break;
				}		
			}
		}
	}

	void MoveToObject(){
		if (_movingToResource || _movingToTown || _movingToFactory){
			float distance = Vector3.Distance(transform.position, _objectPos);
			if (distance <= _range){
				if (_movingToResource){
					_resourceScript.Activate();			
					_movingToResource = false;
				}
				else if (_movingToFactory){
					_factoryScript.EnableFactory();				
					_movingToFactory = false;
				}
				else{
					_townCanvas.OpenCanvas();
					_agent.SetDestination(transform.position);
					_agent.Stop();
					_movingToTown = false;
				}				
				_agent.SetDestination(transform.position);
				_agent.ResetPath();
			}
		}
	}
}