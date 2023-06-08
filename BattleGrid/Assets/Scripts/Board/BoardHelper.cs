using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BoardHelper : MonoBehaviour 
{
	public BoardBuilder Builder;
	public GameObject[,] Board = new GameObject[0,0];
	public bool HasInitialized { get; protected set; }
	public bool BoardHasBeenSetUp
	{
		get
		{
			return ((_tiles != null) && (_tiles.Count > 0) && (_boardTiles != null) && (_boardTiles.Length > 0));
		}
	}

	private GameObject[,] _boardTiles;
	private List<GameObject> _tiles;

	void Awake()
	{
		if(Builder != null)
		{
			this.Builder.TiledCreated += AddTile;
			this.Builder.BoardBuilt += SetUpBoard;
			this.Builder.PositionUpdated += MoveBoard;

			HasInitialized = true;
		}
	}

	public void MoveBoard(Vector3 newPosition)
	{
		Vector3 currentPosition = transform.localPosition;
		Vector3 updatedPosition = new Vector3((currentPosition.x + newPosition.x),(currentPosition.y + newPosition.y),
		                                      (currentPosition.z + newPosition.z));
		transform.localPosition = updatedPosition;
	}

	public void AddTile(GameObject tile)
	{
		if(_tiles == null)
		{
			_tiles = new List<GameObject>();
		}

		if(!_tiles.Contains(tile))
		{
			_tiles.Add(tile);
		}
	}

	public void SetUpBoard()
	{
		int rowCount = 0;
		int columnCount = 0;
		List<List<GameObject>> tilesByRow = new List<List<GameObject>>();

		for(int i = 0; i < _tiles.Count; ++i)
		{
			var currentTile = _tiles[i];
			string[] idParts = currentTile.name.Split(('_'));
			int currentRow = Convert.ToInt32(idParts[idParts.Length - 2]);
			int currentColumn = Convert.ToInt32(idParts[idParts.Length - 1]);
			rowCount += (rowCount < currentRow) ? 1 : 0;
			columnCount += (columnCount < currentColumn) ? 1 : 0;

			List<GameObject> columnsInRow = new List<GameObject>();
			if(tilesByRow.Count == currentRow)
			{
				if(tilesByRow.Count <= currentRow)
				{
					tilesByRow.Insert(currentRow, columnsInRow);
				}

				if(columnsInRow.Count == currentColumn)
				{
					columnsInRow.Insert(currentColumn, currentTile);
				}
			}
			else
			{
				if(tilesByRow[currentRow] != null)
				{
					columnsInRow = tilesByRow[currentRow];
					if(columnsInRow.Count == currentColumn)
					{
						columnsInRow.Insert(currentColumn, currentTile);
					}
				}
			}
		}

		_boardTiles = new GameObject[(rowCount + 1),(columnCount + 1)];

		for(int j = 0; j < tilesByRow.Count; ++j)
		{
			var currentRowOfTiles = tilesByRow[j];
			for(int k = 0; k < currentRowOfTiles.Count; ++k)
			{
				var currentTile = currentRowOfTiles[k];
				_boardTiles[j,k] = currentTile;
			}
		}

		Board = _boardTiles;
	}

	public GameTile GetGameTileFromCoordinates(KeyValuePair<int,int> coordinate)
	{
		GameObject tileObject = Board[coordinate.Key, coordinate.Value];
		GameTile tile = (tileObject != null) ? tileObject.GetComponent<GameTile>() : null;

		return tile;
	}
}
