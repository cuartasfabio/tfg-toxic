using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.TileCreations;
using Gameplay.Visitors.Tiles;
using UnityEngine;

namespace Gameplay.Tiles
{ 
    public abstract class TileBehaviour : MonoBehaviour, ITileVisitorElement
    {
        public ITileEntity TileEntity { get; private set; }

        [SerializeField] private MeshCollider _meshCollider;
        private Tile _ownTile;
        private bool _isPreviewing;
        
        private HexCoordinates _currentCoordinates;

        public void Init(ITileEntity tileEntity, Tile ownTile)
        {
            TileEntity = tileEntity;
            _ownTile = ownTile;
        }

        public virtual void SetCurrentCoordinates(HexCoordinates hexCoordinates)
        {
            _currentCoordinates = hexCoordinates;
        }
        
        public void DeleteSelf()
        {
            Destroy(gameObject);
        }

        public void SetMeshRaycast(bool activate)
        {
            _meshCollider.enabled = activate;
        }

        public void ShowPreview(Color tint)
        {
            if (!_isPreviewing)
            {
                _ownTile.ShowPreview(tint);
                _isPreviewing = true;
            }
        }

        public void ShowTile()
        {
            if (_isPreviewing)
            {
                _ownTile.ShowTile();
                _isPreviewing = false;
            }
        }

        public void EnableView(bool view)
        {
            _ownTile.gameObject.SetActive(view);
        }

        public void RunPlaceAnimation()
        {
            StartCoroutine(_ownTile.TilePlaceAnimation());
        }
        
        public void RunHoverEnterAnimation()
        {
            StartCoroutine(_ownTile.TileHoverEnterAnimation());
        }
        
        public void RunHoverExitAnimation()
        {
           _ownTile.TileHoverExit();
        }

        public abstract void Accept(AbstractTileVisitor tileVisitor);
    }
}


