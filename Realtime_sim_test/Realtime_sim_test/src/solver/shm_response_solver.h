#pragma once
#include <iostream>

class shm_response_solver
{
public:
	const double force_f = 0.0f; // Initial force
	const double i_dipl = 1.0f; // Initial displacement
	const double i_velo = 0.0f; // Initial displacement

	double displ_at_t = 0.0; // displacment at time t
	double velo_at_t = 0.0; // velocity at time t

	shm_response_solver();
	~shm_response_solver();

	void init(double& mass_m, double& stiff_k, double& modaldamp_z);

	void solve_at_time_t(double& time_t, double dt);

private:
	double mass_m = 0.0;
	double stiff_k = 0.0;
	double damp_c = 0.0;

	// Previous time steps (for Adams-Bashforth)
	double prev_displ[4] = { 0.0, 0.0, 0.0, 0.0 };
	double prev_velo[4] = { 0.0, 0.0, 0.0, 0.0 };


	double acceleration_at_timet(double displ_u, double velo_v);



	void rk4_step(double dt);
	void adams_bashforth_step(double dt);
};