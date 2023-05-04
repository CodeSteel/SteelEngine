#version 330 core

layout(location = 0) in vec3 aPos;
layout(location = 1) in vec4 aColor;

out vec4 vertexColor;

uniform mat4 uProjection;
uniform mat4 uModelView;

void main()
{
    gl_Position = uProjection * vec4(aPos, 1.0);
    vertexColor = aColor;
}