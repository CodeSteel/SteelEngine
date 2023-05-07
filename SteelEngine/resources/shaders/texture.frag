#version 330 core

in vec2 texCoord;
in vec4 vertexColor;

out vec4 FragColor;

uniform sampler2D texture0;

void main()
{
    FragColor = texture(texture0, texCoord) * vertexColor;
}