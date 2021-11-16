using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace CodeBase.LevelLogic
{
    public class EnvironmentRoot: MonoBehaviour
    {
        [SerializeField] private EnvironmentDataEvent _onEnvironmentUpdated;
        [SerializeField] private Material _roadRoot;
        [SerializeField] private Material _planeRoot;
        [SerializeField] private Material _rocksRoot;
        [SerializeField] private Material _woodRoot;
        [SerializeField] private Material _castleRoot;
        [SerializeField] private Material _mountains;
        [SerializeField] [ReadOnly] private EnvironmentData _currentEnvironment;
        private void OnEnable() => _onEnvironmentUpdated.AddAction(UpdateEnvironment);

        private void OnDisable() => _onEnvironmentUpdated.RemoveAction(UpdateEnvironment);
        
        
        [Button]
        private void UpdateEnvironment(EnvironmentData data)
        {
            _currentEnvironment = data;
            UpdateMaterials(data);
            UpdateLighting(data.sky, data.equator, data.ground, data.ambientColor, data.source);
            RenderSettings.skybox = data.skybox;
            _mountains.color = data.mountain;
        }

        [Button][ShowIf("EnvironmentIsNotNull")]
        private void SaveCurrentEnvironment()
        {
            _currentEnvironment.road = _roadRoot;
            _currentEnvironment.road.CopyPropertiesFromMaterial(_roadRoot);
            _currentEnvironment.plane = _planeRoot;
            _currentEnvironment.plane.CopyPropertiesFromMaterial(_planeRoot);
            _currentEnvironment.rocks = _rocksRoot;
            _currentEnvironment.rocks.CopyPropertiesFromMaterial(_rocksRoot);
            _currentEnvironment.wood = _woodRoot;
            _currentEnvironment.wood.CopyPropertiesFromMaterial(_woodRoot);
            _currentEnvironment.castle = _castleRoot;
            _currentEnvironment.castle.CopyPropertiesFromMaterial(_castleRoot);
        }

        private bool EnvironmentIsNotNull() => _currentEnvironment != null;

        private void UpdateLighting(Color sky, Color equator, Color ground, Color ambientColor, AmbientMode source)
        {
            RenderSettings.ambientMode = source;
            switch (source)
            {
                case AmbientMode.Trilight:
                    RenderSettings.ambientSkyColor = sky;
                    RenderSettings.ambientEquatorColor = equator;
                    RenderSettings.ambientGroundColor = ground;
                    break;
                case AmbientMode.Flat:
                    RenderSettings.ambientLight = ambientColor;
                    break;
            }
        }

        private void UpdateMaterials(EnvironmentData data)
        {
            _roadRoot.shader = data.road.shader;
            _roadRoot.CopyPropertiesFromMaterial(data.road);
            _planeRoot.shader = data.road.shader;
            _planeRoot.CopyPropertiesFromMaterial(data.plane);
            _rocksRoot.shader = data.road.shader;
            _rocksRoot.CopyPropertiesFromMaterial(data.rocks);
            _woodRoot.shader = data.wood.shader;
            _woodRoot.CopyPropertiesFromMaterial(data.wood);
            _castleRoot.shader = data.castle.shader;
            _castleRoot.CopyPropertiesFromMaterial(data.castle);
        }
    }
}