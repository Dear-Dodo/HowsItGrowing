using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FullscreenPass : ScriptableRendererFeature
{
    

    class Pass : ScriptableRenderPass
    {
        public RenderTargetIdentifier cameraColor { get; set; } //camera render texture

        private RenderTargetHandle m_TemporaryColorTexture;     //temp texture
        private CustomSettings settings;                        //setings object

        public Pass(CustomSettings settings)
        {
            this.settings = settings;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            //Initalise temp texture
            m_TemporaryColorTexture.Init("_TemporaryColorTexture");

            //Texture metadata
            RenderTextureDescriptor desc = cameraTextureDescriptor;
            desc.depthBufferBits = 0;

            cmd.GetTemporaryRT(m_TemporaryColorTexture.id, desc, FilterMode.Point);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            //Get the command buffer
            CommandBuffer cmd = CommandBufferPool.Get("FullscreenPass");
            //Copy texture to temporary RT
            cmd.CopyTexture(cameraColor, m_TemporaryColorTexture.Identifier());
            cmd.SetGlobalTexture(Shader.PropertyToID("_MainTex"), m_TemporaryColorTexture.Identifier());

            //Set Projection & Draw Fullscreen Mesh
            cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
            Matrix4x4 matrix = Matrix4x4.TRS(new Vector3(0, 0, -1), Quaternion.identity, Vector3.one);
            cmd.DrawMesh(RenderingUtils.fullscreenMesh, matrix, settings.material);

            //Reset Projection
            Camera camera = renderingData.cameraData.camera;
            cmd.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (cmd == null)
                throw new System.ArgumentNullException("cmd");

            //Release Temporary RT
            cmd.ReleaseTemporaryRT(m_TemporaryColorTexture.id);
        }
    }

    //inspector settings
    [System.Serializable]
    public class CustomSettings
    {

        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        public Material material;
    }

    public CustomSettings settings = new CustomSettings();

    Pass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new Pass(settings);
        m_ScriptablePass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        m_ScriptablePass.cameraColor = renderer.cameraColorTarget;

        if (settings.material == null)
        {
            return;
        }

        renderer.EnqueuePass(m_ScriptablePass);
    }
}