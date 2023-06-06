import axios from 'axios';
import { Vehicle, PostVehicle } from './dataTypes';
import { BASE_URL } from './baseUrl';

const vehiclesURL = BASE_URL + "/vehicles";

export const vehicleService = {
    addVehicle: async (vehicle: PostVehicle): Promise<Vehicle> => {
      const response = await axios.post<Vehicle>(`${vehiclesURL}/add`, vehicle);
      return response.data;
    },

    getVehiclesByOperator: async (operatorId: string): Promise<Vehicle[]> => {
      const response = await axios.get<Vehicle[]>(`${vehiclesURL}/operator/${operatorId}`);
      return response.data;
    },

    getVehicleById: async (id: string): Promise<Vehicle> => {
      const response = await axios.get<Vehicle>(`${vehiclesURL}/${id}`);
      return response.data;
    },

    editRentStatus: async (id: string, status: boolean): Promise<Vehicle> => {
        const response = await axios.put<Vehicle>(`${vehiclesURL}/${id}/rent?status=${status}`);
      return response.data;
    },

    editRentFee: async (id: string, value: number): Promise<Vehicle> => {
        const response = await axios.put<Vehicle>(`${vehiclesURL}/${id}/fee?value=${value}`);
      return response.data;
    },

    removeVehicle: async (id: string): Promise<boolean> => {
      const response = await axios.delete<boolean>(`${vehiclesURL}/${id}/remove`);
      return response.data;
    }
};
