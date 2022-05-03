using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Blit : ScriptableRendererFeature
{

    public class BlitPass : ScriptableRenderPass
    {
        public enum RenderTarget
        {
            Color,
            RenderTexture,
        }

        public Material blitMaterial = null;                    //Material to blit with
        public int blitShaderPassIndex = 0;                     //Shader pass to use
        public FilterMode filterMode { get; set; }              //texture filter mode

        private RenderTargetIdentifier source { get; set; }     //Target to blit from
        private RenderTargetHandle destination { get; set; }    //Target to blit to

        RenderTargetHandle m_TemporaryColorTexture;             //Temp texture
        string m_ProfilerTag;                                   //Debug profiler tag
        public BlitPass(RenderPassEvent renderPassEvent, Material blitMaterial, int blitShaderPassIndex, string tag)
        {
            this.renderPassEvent = renderPassEvent;
            this.blitMaterial = blitMaterial;
            this.blitShaderPassIndex = blitShaderPassIndex;
            m_ProfilerTag = tag;
            m_TemporaryColorTexture.Init("_TemporaryColorTexture");
        }

        //Setup phase, set source and dest
        public void Setup(RenderTargetIdentifier source, RenderTargetHandle destination)
        {
            this.source = source;
            this.destination = destination;
        }

        //Execute render pass
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            //Get the command buffer
            CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);

            //Texture metadata
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 24;

            // Can't read and write to same color target, use a temporary renderTexture
            if (destination == RenderTargetHandle.CameraTarget)
            {
                cmd.GetTemporaryRT(m_TemporaryColorTexture.id, desc, filterMode);
                Blit(cmd, source, m_TemporaryColorTexture.Identifier(), blitMaterial, blitShaderPassIndex);
                Blit(cmd, m_TemporaryColorTexture.Identifier(), source);
            }
            else
            {
                Blit(cmd, source, destination.Identifier(), blitMaterial, blitShaderPassIndex);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
            
        }

        //After executing
        public override void FrameCleanup(CommandBuffer cmd)
        {
            //release any temporary textures
            if (destination == RenderTargetHandle.CameraTarget)
                cmd.ReleaseTemporaryRT(m_TemporaryColorTexture.id);
        }
    }

    //inspector settings
    [System.Serializable]
    public class BlitSettings
    {
        public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

        public Material blitMaterial = null;
        public int blitMaterialPassIndex = 0;
        public Target destination = Target.Color;
        public string textureId = "_BlitPassTexture";
    }

    public enum Target
    {
        Color,
        Texture
    }

    public BlitSettings settings = new BlitSettings();
    RenderTargetHandle m_RenderTextureHandle;

    BlitPass blitPass;

    public override void Create()
    {
        //set shader and material pass indexes
        var passIndex = settings.blitMaterial != null ? settings.blitMaterial.passCount - 1 : 1;
        settings.blitMaterialPassIndex = Mathf.Clamp(settings.blitMaterialPassIndex, -1, passIndex);
        blitPass = new BlitPass(settings.Event, settings.blitMaterial, settings.blitMaterialPassIndex, name);
        m_RenderTextureHandle.Init(settings.textureId);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var src = renderer.cameraColorTarget;
        var dest = (settings.destination == Target.Color) ? RenderTargetHandle.CameraTarget : m_RenderTextureHandle;

        if (settings.blitMaterial == null)
        {
            Debug.LogWarningFormat("Missing Blit Material: {0}", GetType().Name);
            return;
        }

        blitPass.Setup(src, dest);
        renderer.EnqueuePass(blitPass);
    }
}