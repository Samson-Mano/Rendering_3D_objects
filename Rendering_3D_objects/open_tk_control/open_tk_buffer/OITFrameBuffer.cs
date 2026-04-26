
using Rendering_3D_objects.open_tk_control.open_tk_shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// OpenTK library
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;


namespace Rendering_3D_objects.open_tk_control.open_tk_buffer
{

    public class OITFrameBuffer
    {

        private int oitFrameBufferId;
        private int accumulationTextureId;
        private int revealageTextureId;
        private int depthTextureId;

        private int width;
        private int height;
        private Shader resolutionShader;
        private FullscreenQuad fullscreenQuad;

        public OITFrameBuffer(int width, int height)
        {
            this.width = width;
            this.height = height;

            // Create resolution shader
            resolutionShader = new Shader(
                shader_store.oit_resolution_vert_shader(),
                shader_store.oit_resolution_frag_shader()
            );

            CreateFramebuffer();
        }


        public void Resize(int newWidth, int newHeight)
        {
            if (newWidth == width && newHeight == height)
                return;

            width = newWidth;
            height = newHeight;

            // Recreate textures with new size
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, oitFrameBufferId);

            // Resize accumulation texture
            GL.BindTexture(TextureTarget.Texture2D, accumulationTextureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f,
                width, height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);

            // Resize revealage texture
            GL.BindTexture(TextureTarget.Texture2D, revealageTextureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R32f,
                width, height, 0, PixelFormat.Red, PixelType.Float, IntPtr.Zero);

            // Resize depth texture
            GL.BindTexture(TextureTarget.Texture2D, depthTextureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent24,
                width, height, 0, PixelFormat.DepthComponent, PixelType.UnsignedInt, IntPtr.Zero);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }


        private void CreateFramebuffer()
        {

            // Create fullscreen quad for composition
            fullscreenQuad = new FullscreenQuad();

            // Create the OIT Frame Buffer (your existing code)
            oitFrameBufferId = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, oitFrameBufferId);

            // -------------------------
            // Accumulation buffer (RGBA16F/32F)
            // -------------------------
            // Accumulation texture (RGBA)
            accumulationTextureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, accumulationTextureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f,
                width, height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                            (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                            (int)TextureMagFilter.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D,
                accumulationTextureId, 0);

            // -------------------------
            // Revealage buffer (R32F)
            // -------------------------
            // Revealage texture (R)
            revealageTextureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, revealageTextureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R32f,
                width, height, 0, PixelFormat.Red, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                            (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                            (int)TextureMagFilter.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D,
                revealageTextureId, 0);

            // -------------------------
            // Depth buffer
            // -------------------------
            // Depth texture
            depthTextureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, depthTextureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent24,
                width, height, 0, PixelFormat.DepthComponent, PixelType.UnsignedInt, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                            (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                            (int)TextureMagFilter.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D,
                depthTextureId, 0);

            // Draw buffers
            DrawBuffersEnum[] drawBuffers = new DrawBuffersEnum[]
            {
            DrawBuffersEnum.ColorAttachment0,
            DrawBuffersEnum.ColorAttachment1
            };

            GL.DrawBuffers(drawBuffers.Length, drawBuffers);

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) !=
                FramebufferErrorCode.FramebufferComplete)
                throw new Exception("OIT FBO not complete");

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, oitFrameBufferId);
            GL.Viewport(0, 0, width, height);


            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(false);



            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            // GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);


            //// Draw buffers
            //DrawBuffersEnum[] drawBuffers = new DrawBuffersEnum[]
            //{
            //DrawBuffersEnum.ColorAttachment0,
            //DrawBuffersEnum.ColorAttachment1
            //};

            //GL.DrawBuffers(drawBuffers.Length, drawBuffers);


        }

        public void UnBind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Clear(Vector4 clearColor)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, oitFrameBufferId);

            // Clear accumulation to (0,0,0,0)
            GL.ClearBuffer(ClearBuffer.Color, 0, new float[] { 0, 0, 0, 0 }); // accum
            // Clear revealage to 1.0 (fully opaque/empty)
            GL.ClearBuffer(ClearBuffer.Color, 1, new float[] { 1.0f });      // reveal
            // Clear depth
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }

        public void ResolveToScreen()
        {
            // Unbind OIT framebuffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            // Make sure we're rendering to the default framebuffer
            GL.Viewport(0, 0, width, height);

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);

            // Use resolution shader
            resolutionShader.Use();

            // Bind textures
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, accumulationTextureId);
            resolutionShader.SetInt("uAccumulationTexture", 0);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, revealageTextureId);
            resolutionShader.SetInt("uRevealageTexture", 1);

            // Draw fullscreen quad
            fullscreenQuad.Draw();
        }
    }
}