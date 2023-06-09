#version 330 core

in vec4 vertexColor;
in vec2 texCoord;

out vec4 FragColor;

uniform sampler2D texture0;

void main()
{
    vec4 texColor = texture(texture0, texCoord);
    if (texCoord == vec2(1, 1)) {
        FragColor = vertexColor;
    } else {
        FragColor = vec4(vertexColor.rgb, vertexColor.a * texColor.a) * texColor;
    }
    
}