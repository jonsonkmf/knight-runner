using ScriptableSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace CodeBase.LevelLogic
{
    [InlineEditor()]
    [CreateAssetMenu(
        menuName = SOConstants.DataSubmenu + "Environment",
        fileName = "EnvironmentData",
        order = SOConstants.AssetMenuOrder)]
    public class EnvironmentData : ScriptableObject
    {
        [Title("Skybox")] [SerializeField] private Material _skybox;

        [Title("Materials")] [SerializeField] private Material _road;
        [SerializeField] private Material _plane;
        [SerializeField] private Material _rocks;
        [SerializeField] private Material _wood;
        [SerializeField] private Material _castle;

        [SerializeField] private AmbientMode _source = AmbientMode.Flat;

        [ShowIfGroup("GradientLightning/_source",Value = AmbientMode.Trilight)]
        [BoxGroup("GradientLightning")] 
        [SerializeField] private Color _sky;
        [ShowIfGroup("GradientLightning/_source",Value = AmbientMode.Trilight)]
        [BoxGroup("GradientLightning")] 
        [SerializeField] private Color _equator;
        [ShowIfGroup("GradientLightning/_source",Value = AmbientMode.Trilight)]
        [BoxGroup("GradientLightning")] 
        [SerializeField] private Color _ground;

        [ShowIfGroup("ColorLightning/_source", Value = AmbientMode.Flat)]
        [BoxGroup("ColorLightning")]
        [SerializeField] private Color _ambientColor;


        [Title("Mountains")] [SerializeField] private Color _mountain;

        [Button]
        private void SaveCurrentLighting()
        {
            switch (source)
            {
                case AmbientMode.Flat:
                    _ambientColor = RenderSettings.ambientLight;
                    break;
                case AmbientMode.Trilight:
                    _sky = RenderSettings.ambientSkyColor;
                    _equator = RenderSettings.ambientEquatorColor;
                    _ground = RenderSettings.ambientGroundColor;
                    break;
            }
        }

        [Button]
        public void SaveColor(Material material) => _mountain = material.color;

        public Material road
        {
            get => _road;
            set => _road = value;
        }

        public Material plane
        {
            get => _plane;
            set => _plane = value;
        }

        public Material rocks
        {
            get => _rocks;
            set => _rocks = value;
        }

        public Material wood
        {
            get => _wood;
            set => _wood = value;
        }

        public Material skybox => _skybox;

        public Color sky => _sky;

        public Color equator => _equator;

        public Color ground => _ground;

        public Color mountain => _mountain;

        public AmbientMode source => _source;

        public Color ambientColor => _ambientColor;

        public Material castle
        {
            get => _castle;
            set => _castle = value;
        }
    }


  
}