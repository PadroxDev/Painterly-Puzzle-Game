using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

namespace Padrox
{
    public class OilPaintingEffect : ScriptableRendererFeature {
        public Settings settings;

        private OilPaintingEffectPass renderPass;

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
            renderer.EnqueuePass(renderPass);
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData) {
            renderPass.Setup(settings);
        }

        public override void Create() {
            var structureTensorMaterial = CoreUtils.CreateEngineMaterial("Hidden/Oil Painting/Structure Tensor");
            
            renderPass = new OilPaintingEffectPass(structureTensorMaterial);
            renderPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        [Serializable]
        public class Settings {

        }
    }
}
