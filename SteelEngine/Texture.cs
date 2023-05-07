using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using StbImageSharp;
using System.Runtime.InteropServices;

namespace SteelEngine
{
    public class Texture
    {
        public readonly int Handle;
        public readonly byte[] Data;

        public static Texture LoadFromFile(string path)
        {
            int handle = GL.GenTexture();
            byte[] imageData;

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            StbImage.stbi_set_flip_vertically_on_load(1);

            using (Stream stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                imageData = image.Data;
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(handle, imageData);
        }

        public static Texture FromBitmap(Bitmap bitmap)
        {
            // Lock the bitmap to get access to the image data
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Create a new Texture object with the image data
            Texture texture = new Texture(bitmap.Width, bitmap.Height, new byte[bitmapData.Stride * bitmap.Height]);
            Marshal.Copy(bitmapData.Scan0, texture.Data, 0, texture.Data.Length);

            // Unlock the bitmap
            bitmap.UnlockBits(bitmapData);

            return texture;
        }

        public Texture(int glHandle)
        {
            Handle = glHandle;
        }

        public Texture(int glHandle, byte[] data)
        {
            Handle = glHandle;
            Data = data;
        }

        public Texture(int width, int height, byte[] data)
        {
            Handle = GL.GenTexture();
            Data = data;

            // Bind texture ID to Texture2D target
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            // Upload image data to texture
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

            // Set texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        }

        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
