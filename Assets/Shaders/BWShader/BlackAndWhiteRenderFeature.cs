using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlackAndWhiteRenderFeature : ScriptableRendererFeature
{
    class BlackAndWhitePass : ScriptableRenderPass
    {
        private Material material;

        public BlackAndWhitePass(Material material)
        {
            this.material = material;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("BlackAndWhitePass");

            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTargetHandle;
            RenderTargetIdentifier destination = source;

            Blit(cmd, source, destination, material);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [System.Serializable]
    public class BlackAndWhiteSettings
    {
        public Material material;
    }

    public BlackAndWhiteSettings settings = new BlackAndWhiteSettings();
    private BlackAndWhitePass pass;

    public override void Create()
    {
        pass = new BlackAndWhitePass(settings.material);
        pass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.material != null)
        {
            renderer.EnqueuePass(pass);
        }
    }
}
