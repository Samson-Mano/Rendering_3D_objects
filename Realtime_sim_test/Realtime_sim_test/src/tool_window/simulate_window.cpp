#include "simulate_window.h"

simulate_window::simulate_window()
{
	// Empty constructor
}

simulate_window::~simulate_window()
{
	// Empty destructor
}

void simulate_window::init()
{
	is_show_window = false;
	execute_update_model = false;

}

void simulate_window::render_window()
{
    if (is_show_window == false)
    {
        return;
    }

    static float mass = 1.0f;        // Default values
    static float stiffness = 100.0f;
    static float damping_ratio = 0.05f;
    bool is_input_valid = true;

    // Create a new ImGui window
    ImGui::Begin("Simulation Setup");

    // Input box to get Mass
    ImGui::InputFloat("Mass (kg)", &mass);

    // Input box to get Stiffness
    ImGui::InputFloat("Stiffness (N/m)", &stiffness);

    // Input box to get Damping Ratio
    ImGui::InputFloat("Damping Ratio", &damping_ratio);

    // Check if inputs are valid to avoid division by zero
    if (mass > 0 && stiffness > 0)
    {
        // Compute the angular natural frequency w_n
        float w_n = sqrt(stiffness / mass); // w_n = sqrt(k/m)

        // Compute the natural frequency f_n
        float f_n = w_n / (2.0f * 3.14159265f); // f_n = w_n / (2 * pi)

        // Compute the natural period T_n
        float T_n = 1.0f / f_n; // T_n = 1 / f_n

        // Print the angular natural frequency w_n
        ImGui::Text("Angular Natural Frequency (w_n): %.3f rad/s", w_n);

        // Print the natural frequency f_n
        ImGui::Text("Natural Frequency (f_n): %.3f Hz", f_n);

        // Print the natural period T_n
        ImGui::Text("Natural Period (T_n): %.3f s", T_n);
    }
    else
    {
        ImGui::Text("Invalid input: Mass and stiffness must be positive.");

        is_input_valid = false;
    }

    // Add an "Update model" button
    if (ImGui::Button("Update model"))
    {
        // Is Input valid
        if (is_input_valid == true)
        {
            // Store the model parameters
            this->mass_m = mass;
            this->stiffness_k = stiffness;
            this->damping_ratio_si = damping_ratio;

            execute_update_model = true;
        }
    }

    // Add a "Close" button
    if (ImGui::Button("Close"))
    {
        is_show_window = false;
    }

    ImGui::End();

}
