using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using StbImageSharp;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;

namespace SteelEngine
{
    public class Texture : IDisposable
    {
        public readonly int Handle;
        public readonly byte[] Data;
        public readonly int Width;
        public readonly int Height;

        /// <summary>
        /// Creates a texture from a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture LoadFromFile(string path)
        {
            byte[] imageData;
            int width;
            int height;

            int handle = GL.GenTexture();

            if (handle == 0)
            {
                throw new Exception("Failed to generate texture object.");
            }

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            StbImage.stbi_set_flip_vertically_on_load(1);

            using (Stream stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                if (image == null)
                {
                    throw new Exception("Failed to load image from file.");
                }

                imageData = image.Data;

                if (imageData == null)
                {
                    throw new Exception("Failed to load image data from file.");
                }

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, imageData);

                width = image.Width;
                height = image.Height;
            }

            // Set texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            // GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            // Check for errors
            ErrorCode errorCode = GL.GetError();
            if (errorCode != ErrorCode.NoError)
            {
                throw new Exception("OpenGL error: " + errorCode.ToString());
            }

            return new Texture(handle, imageData, width, height);
        }

        /// <summary>
        /// Creates a texture from a bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Texture FromBitmap(Bitmap bitmap)
        {
            if (bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                throw new System.Exception("Bitmap is not in 32bppArgb format!");
            }

            // Lock the bitmap to get access to the image data
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            if (bitmapData == null)
            {
                throw new System.Exception("Could not handle the bitmap data!");
            }

            // Create a new Texture object with the image data
            byte[] textureData = new byte[bitmapData.Stride * bitmap.Height];
            Marshal.Copy(bitmapData.Scan0, textureData, 0, bitmapData.Stride * bitmap.Height);
            Texture texture = new Texture(bitmap.Width, bitmap.Height, textureData);

            // Unlock the bitmap
            bitmap.UnlockBits(bitmapData);

            return texture;
        }

        public Texture(int glHandle)
        {
            Handle = glHandle;
        }

        public Texture(int glHandle, byte[] data, int width, int height)
        {
            Handle = glHandle;
            Data = data;
            Width = width;
            Height = height;
        }

        public Texture(int width, int height, byte[] data)
        {
            Handle = GL.GenTexture();
            Data = data;
            Width = width;
            Height = height;

            // Bind texture ID to Texture2D target
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            // Upload image data to texture
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

            // Set texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Check for errors
            ErrorCode errorCode = GL.GetError();
            if (errorCode != ErrorCode.NoError)
            {
                throw new Exception("OpenGL error: " + errorCode.ToString());
            }
        }

        public void Dispose()
        {
            GL.DeleteTexture(Handle);
        }

        /// <summary>
        /// Uses the Texture in the current GL scope.
        /// </summary>
        /// <param name="unit"></param>
        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
