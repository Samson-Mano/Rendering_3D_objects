#include "shm_response_solver.h"

shm_response_solver::shm_response_solver()
{
}

shm_response_solver::~shm_response_solver()
{
}

void shm_response_solver::init(double& mass_m, double& stiff_k, double& modaldamp_z)
{
	this->mass_m = mass_m;
	this->stiff_k = stiff_k;
	this->damp_c = modaldamp_z * 2.0 * sqrt(mass_m * stiff_k);  // Damping coefficient based on modal damping

	// Initial displacement and velocity
	this->displ_at_t = i_dipl;
	this->velo_at_t = i_velo;

}


double shm_response_solver::acceleration_at_timet(double displ_u, double velo_v)
{
	return (1.0 / this->mass_m) * (force_f - damp_c * velo_v - stiff_k * displ_u);
}


void shm_response_solver::solve_at_time_t(double& time_t, double dt)
{
	// If we are in the first few steps, use RK4
	if (time_t < 4 * dt)
	{
		rk4_step(dt);  // Use RK4 method
	}
	else
	{
		adams_bashforth_step(dt);  // After enough steps, switch to Adams-Bashforth
	}

}


void shm_response_solver::rk4_step(double dt)
{
	// k1
	double k1_velo = velo_at_t;
	double k1_accel = acceleration_at_timet(displ_at_t, velo_at_t);

	// k2
	double k2_velo = velo_at_t + 0.5 * dt * k1_accel;
	double k2_accel = acceleration_at_timet(displ_at_t + 0.5 * dt * k1_velo, velo_at_t + 0.5 * dt * k1_accel);

	// k3
	double k3_velo = velo_at_t + 0.5 * dt * k2_accel;
	double k3_accel = acceleration_at_timet(displ_at_t + 0.5 * dt * k2_velo, velo_at_t + 0.5 * dt * k2_accel);

	// k4
	double k4_velo = velo_at_t + dt * k3_accel;
	double k4_accel = acceleration_at_timet(displ_at_t + dt * k3_velo, velo_at_t + dt * k3_accel);

	// Update displacement and velocity using RK4 formula
	displ_at_t += (dt / 6.0) * (k1_velo + 2.0 * k2_velo + 2.0 * k3_velo + k4_velo);
	velo_at_t += (dt / 6.0) * (k1_accel + 2.0 * k2_accel + 2.0 * k3_accel + k4_accel);
}

void shm_response_solver::adams_bashforth_step(double dt)
{
	// Use previous displacements and velocities for multi-step Adams-Bashforth
	double accel_t = acceleration_at_timet(displ_at_t, velo_at_t);
	double accel_prev1 = acceleration_at_timet(prev_displ[0], prev_velo[0]);
	double accel_prev2 = acceleration_at_timet(prev_displ[1], prev_velo[1]);
	double accel_prev3 = acceleration_at_timet(prev_displ[2], prev_velo[2]);

	// Adams-Bashforth 4-step method for velocity
	velo_at_t += (dt / 24.0) * (55 * accel_t - 59 * accel_prev1 + 37 * accel_prev2 - 9 * accel_prev3);

	// Update displacement using new velocity
	displ_at_t += velo_at_t * dt;

	// Shift the previous states
	for (int i = 3; i > 0; --i) 
	{
		prev_displ[i] = prev_displ[i - 1];
		prev_velo[i] = prev_velo[i - 1];
	}
	prev_displ[0] = displ_at_t;
	prev_velo[0] = velo_at_t;

}
