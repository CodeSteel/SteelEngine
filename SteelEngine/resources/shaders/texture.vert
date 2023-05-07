#version 330 core

layout(location = 0) in vec2 aPos;
layout(location = 1) in vec2 aTexCoord;
layout(location = 2) in vec4 aColor;

out vec4 vertexColor;
out vec2 texCoord;

uniform mat4 uProjection;
uniform mat4 uModelView;

void main()
{
    vertexColor = aColor;
    texCoord = aTexCoord;
    gl_Position = uProjection * vec4(aPos, 0.0, 1.0);
}