#include "matrices_window.h"

matrices_window::matrices_window()
{
	// Empty constructor
}

matrices_window::~matrices_window()
{
	// Empty destructor
}

void matrices_window::init(geom_parameters* geom_param_ptr)
{
	is_show_window = false;

	// Set the geometry parameters
	this->geom_param_ptr = geom_param_ptr;

}

// Function to convert a mat4 to a formatted string for display or copying
std::string matrices_window::formatMat4ToString(const glm::mat4& matrix)
{
    std::string matrixString;
    for (int row = 0; row < 4; ++row)
    {
        for (int col = 0; col < 4; ++col)
        {
            char element[32]; // Temporary buffer for each matrix element
            snprintf(element, sizeof(element), "%.8f", matrix[row][col]); // Format each element with 8 decimal places

            matrixString += element;
            if (col < 3) matrixString += "\t"; // Add a tab between elements in the same row
        }
        matrixString += "\n"; // Add a newline at the end of each row
    }

    return matrixString; // Return the formatted string
}

void matrices_window::render_window()
{
    if (is_show_window == false)
        return;

    // Create a new ImGui window
    ImGui::Begin("View Matrices");

    // Create a buffer large enough to hold the entire matrix as a string
    char matrixBuffer[512]; // Adjust size as needed, 512 should be sufficient for a 4x4 matrix with formatting

    // Concatenate matrix elements into a single string
    std::string matrixString = formatMat4ToString(geom_param_ptr->modelMatrix);

    // Copy the constructed matrix string to the buffer
    strncpy_s(matrixBuffer, matrixString.c_str(), sizeof(matrixBuffer) - 1);

    // Ensure buffer is null-terminated
    matrixBuffer[sizeof(matrixBuffer) - 1] = '\0';


    // Set custom width and height for the text box
    float customWidth = 400.0f; // You can adjust this value
    float customHeight = ImGui::GetTextLineHeight() * 5.5; // Adjust height as needed

    // Print Model Matrix
    char modelMatrixBuffer[512]; // Adjust size as needed
    std::string modelMatrixString = formatMat4ToString(geom_param_ptr->modelMatrix);
    strncpy_s(modelMatrixBuffer, modelMatrixString.c_str(), sizeof(modelMatrixBuffer) - 1);
    modelMatrixBuffer[sizeof(modelMatrixBuffer) - 1] = '\0';

    ImGui::Text("Model Matrix:");
    ImGui::InputTextMultiline("##modelMatrix", modelMatrixBuffer, sizeof(modelMatrixBuffer), ImVec2(customWidth, customHeight), ImGuiInputTextFlags_ReadOnly);

    ImGui::Separator(); // Separator line for UI organization

    // Print Rotation Matrix
    char rotationMatrixBuffer[512]; // Adjust size as needed
    std::string rotationMatrixString = formatMat4ToString(geom_param_ptr->rotateTranslation);
    strncpy_s(rotationMatrixBuffer, rotationMatrixString.c_str(), sizeof(rotationMatrixBuffer) - 1);
    rotationMatrixBuffer[sizeof(rotationMatrixBuffer) - 1] = '\0';

    ImGui::Text("Rotation Matrix:");
    ImGui::InputTextMultiline("##rotationMatrix", rotationMatrixBuffer, sizeof(rotationMatrixBuffer), ImVec2(customWidth, customHeight), ImGuiInputTextFlags_ReadOnly);

    ImGui::Separator();


    // Print Pan Translation Matrix
    glm::mat4 panTranslationMatrix = geom_param_ptr->panTranslation;


    char pantranslMatrixBuffer[512]; // Adjust size as needed
    std::string pantransMatrixString = formatMat4ToString(panTranslationMatrix);
    strncpy_s(pantranslMatrixBuffer, pantransMatrixString.c_str(), sizeof(pantranslMatrixBuffer) - 1);
    pantranslMatrixBuffer[sizeof(pantranslMatrixBuffer) - 1] = '\0';

    ImGui::Text("Pan Translation Matrix:");
    ImGui::InputTextMultiline("##pantranslMatrix", pantranslMatrixBuffer, sizeof(pantranslMatrixBuffer), ImVec2(customWidth, customHeight), ImGuiInputTextFlags_ReadOnly);

    ImGui::Separator();


    // Print Scaling Matrix
    glm::mat4 scalingMatrix = glm::mat4(1.0) * static_cast<float>(geom_param_ptr->zoom_scale);
    scalingMatrix[3][3] = 1.0f;

    char scalingMatrixBuffer[512]; // Adjust size as needed
    std::string scalingMatrixString = formatMat4ToString(scalingMatrix);
    strncpy_s(scalingMatrixBuffer, scalingMatrixString.c_str(), sizeof(scalingMatrixBuffer) - 1);
    scalingMatrixBuffer[sizeof(scalingMatrixBuffer) - 1] = '\0';

    ImGui::Text("Scaling Matrix:");
    ImGui::InputTextMultiline("##scalingMatrix", scalingMatrixBuffer, sizeof(scalingMatrixBuffer), ImVec2(customWidth, customHeight), ImGuiInputTextFlags_ReadOnly);

    ImGui::Separator();


    // Print View Matrix
    glm::mat4 viewMatrix = geom_param_ptr->rotateTranslation * scalingMatrix;
    
    char viewMatrixBuffer[512]; // Adjust size as needed
    std::string viewMatrixString = formatMat4ToString(viewMatrix);
    strncpy_s(viewMatrixBuffer, viewMatrixString.c_str(), sizeof(viewMatrixBuffer) - 1);
    viewMatrixBuffer[sizeof(viewMatrixBuffer) - 1] = '\0';

    ImGui::Text("View Matrix:");
    ImGui::InputTextMultiline("##viewMatrix", viewMatrixBuffer, sizeof(viewMatrixBuffer), ImVec2(customWidth, customHeight), ImGuiInputTextFlags_ReadOnly);

    ImGui::Separator();
    

    // Print View x Model Matrix
    glm::mat4 viewmodelMatrix = geom_param_ptr->rotateTranslation * scalingMatrix * geom_param_ptr->modelMatrix;
    
    char viewmodelMatrixBuffer[512]; // Adjust size as needed
    std::string viewmodelMatrixString = formatMat4ToString(viewmodelMatrix);
    strncpy_s(viewmodelMatrixBuffer, viewmodelMatrixString.c_str(), sizeof(viewmodelMatrixBuffer) - 1);
    viewmodelMatrixBuffer[sizeof(viewmodelMatrixBuffer) - 1] = '\0';

    ImGui::Text("View x Model Matrix:");
    ImGui::InputTextMultiline("##viewmodelMatrix", viewmodelMatrixBuffer, sizeof(viewmodelMatrixBuffer), ImVec2(customWidth, customHeight), ImGuiInputTextFlags_ReadOnly);

    ImGui::Separator();


    // Print transpose(panTranslation) x View x Model Matrix
    glm::mat4 panviewmodelMatrix = glm::transpose(panTranslationMatrix) *  viewmodelMatrix;

    char panviewmodelMatrixBuffer[512]; // Adjust size as needed
    std::string panviewmodelMatrixString = formatMat4ToString(panviewmodelMatrix);
    strncpy_s(panviewmodelMatrixBuffer, panviewmodelMatrixString.c_str(), sizeof(panviewmodelMatrixBuffer) - 1);
    panviewmodelMatrixBuffer[sizeof(panviewmodelMatrixBuffer) - 1] = '\0';

    ImGui::Text("Pan^T x View x Model Matrix:");
    ImGui::InputTextMultiline("##panviewmodelMatrix", panviewmodelMatrixBuffer, sizeof(panviewmodelMatrixBuffer), ImVec2(customWidth, customHeight), ImGuiInputTextFlags_ReadOnly);

    ImGui::Separator();



    // Add a "Close" button
    if (ImGui::Button("Close"))
    {
        is_show_window = false;
    }

    ImGui::End();

}
