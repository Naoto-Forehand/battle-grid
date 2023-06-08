using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BoardBuilder : MonoBehaviour 
{
	public GameObject Tile_Prefab;
	public GameObject Board_Container;
	public int Divider = 3;
	public int Grid_Size = 0;
	public int Grid_Columns = 0;

	private BoardHelper _helper;
	private Bounds _tileBounds;
	private float _xOffset;
	private float _yOffset;

	public delegate void OnBuilderReady();
	public event OnBuilderReady BuilderReady;

	public delegate void OnTileCreated(GameObject tile);
	public event OnTileCreated TiledCreated;

	public delegate void OnBoardBuilt();
	public event OnBoardBuilt BoardBuilt;

	public delegate void OnPositionUpdate(Vector3 newPosition);
	public event OnPositionUpdate PositionUpdated;

	void Awake()
	{
		var renderer = Tile_Prefab.GetComponent<SpriteRenderer>();
		if(renderer != null)
		{
			_tileBounds = renderer.bounds;
			var minX = (_tileBounds.min.x < 0f) ? (_tileBounds.min.x * -1f) : (_tileBounds.min.x);
			var maxX = (_tileBounds.max.x < 0f) ? (_tileBounds.max.x * -1f) : (_tileBounds.max.x);

			_xOffset = (minX + maxX);

			var minY = (_tileBounds.min.y < 0f) ? (_tileBounds.min.y * -1f) : (_tileBounds.min.y);
			var maxY = (_tileBounds.max.y < 0f) ? (_tileBounds.max.y * -1f) : (_tileBounds.max.y);
			_yOffset = (minY + maxY);
		}

		if(this.Board_Container != null)
		{
			_helper = this.Board_Container.GetComponent<BoardHelper>();
		}

		if (this.BuilderReady != null)
        {
			this.BuilderReady();
        }
	}

	public void BuildBoard()
	{
		Action onComplete = delegate()
		{
			if((Grid_Size % Divider) == 0)
			{
				int rowsToBuild = Grid_Size / Divider;

				for(int i = 0; i < rowsToBuild; ++i)
				{
					for(int j = 0; j < Grid_Columns; ++j)
					{
						string newName = string.Format("Tile_{0}_{1}", i, j);
						GameObject tile = GameObject.Instantiate(Tile_Prefab) as GameObject;
						tile.name = newName;
						tile.transform.SetParent(Board_Container.transform);
						Vector3 prefabPosition = Tile_Prefab.transform.position;
						Vector3 newPosition = new Vector3((prefabPosition.x + (_xOffset * j)), (prefabPosition.y + (_yOffset * i)), prefabPosition.z);
						tile.transform.position = newPosition;

						if(TiledCreated != null)
						{
							TiledCreated(tile);
						}
					}
				}

				if(BoardBuilt != null)
				{
					BoardBuilt();
				}
			}
		};

		StartCoroutine(ExecuteAfterInit(onComplete));
	}

	public void AdjustPosition(KeyValuePair<float,float> stepDirection)
	{
		float xSize = (Divider * _xOffset);
		float ySize = (Divider * _yOffset);

		Vector3 newPosition = new Vector3((xSize * stepDirection.Key), (ySize * stepDirection.Value), 0f);

		if(PositionUpdated != null)
		{
			PositionUpdated(newPosition);
		}
	}

	public Transform GetTile(KeyValuePair<int,int> location)
	{
		var helper = Board_Container.GetComponent<BoardHelper>();
		if(helper.Board.Length < (location.Key * location.Value))
		{
			var tileObject = helper.Board[location.Key, location.Value];
			return tileObject.transform;
		}

		return null;
	}

	private IEnumerator ExecuteAfterInit(Action onComplete)
	{
		while(!_helper.HasInitialized)
		{
			yield return null;
		}

		if(onComplete != null)
		{
			onComplete();
		}
	}

	void Update()
	{
		// Add test function hooks here
	}
}
