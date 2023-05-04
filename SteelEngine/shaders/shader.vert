#version 330 core

layout(location = 0) in vec3 aPos;
layout(location = 1) in vec4 aColor;

uniform mat4 u_projection;

out vec4 vertexColor;

void main()
{
    gl_Position = u_projection * vec4(aPos, 0.0, 1.0);
    vertexColor = aColor;
}