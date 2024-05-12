using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace Padrox
{
    public class OilPaintingEffectPass : ScriptableRenderPass {
        private RenderTargetIdentifier source;
        private RenderTargetIdentifier destination;

        private RenderTexture structureTensorTex;
        private readonly Material structureTensorMaterial;

        public OilPaintingEffectPass(Material structureTensorMaterial)
        {
            this.structureTensorMaterial = structureTensorMaterial;
        }

        public void Setup(OilPaintingEffect.Settings settings) {

        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) {
            RenderTextureDescriptor blitTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            blitTargetDescriptor.depthBufferBits = 0;

            var renderer = renderingData.cameraData.renderer;

            source = renderer.cameraColorTarget;
            destination = renderer.cameraColorTarget;

            structureTensorTex = RenderTexture.GetTemporary(blitTargetDescriptor.width, blitTargetDescriptor.height, 0, RenderTextureFormat.ARGBFloat);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
            CommandBuffer cmd = CommandBufferPool.Get("Oil Painting Effect");

            Blit(cmd, source, structureTensorTex, structureTensorMaterial, -1);
            Blit(cmd, structureTensorTex, destination);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd) {
            RenderTexture.ReleaseTemporary(structureTensorTex);
        }
    }
}
