import axios from 'axios';
import { Vehicle, PostVehicle } from './dataTypes';
import { BASE_URL } from './baseUrl';

const vehiclesURL = BASE_URL + "/vehicles";

export const vehicleService = {
    addVehicle: async (vehicle: PostVehicle): Promise<Vehicle> => {
      const response = await axios.post<Vehicle>(`${vehiclesURL}/add`, vehicle);
      return response.data;
    },

    getAllVehicles: async (): Promise<Vehicle[]> => {
      const response = await axios.get<Vehicle[]>(vehiclesURL);
      return response.data;
    },

    getVehiclesByOperator: async (operatorId: string): Promise<Vehicle[]> => {
      const response = await axios.get<Vehicle[]>(`${vehiclesURL}/by/op/${operatorId}`);
      return response.data;
    },

    getVehicleById: async (id: string): Promise<Vehicle> => {
      const response = await axios.get<Vehicle>(`${vehiclesURL}/${id}`);
      return response.data;
    },

    getVehicleByPlateNumber: async (plateNumber: string): Promise<Vehicle> => {
      const response = await axios.get<Vehicle>(`${vehiclesURL}/pn/${plateNumber}`);
      return response.data;
    },

    editRentStatus: async (id: string, rentStatus: boolean): Promise<Vehicle> => {
      const response = await axios.put<Vehicle>(`${vehiclesURL}/${id}/status/edit?rentStatus=${rentStatus}`);
      return response.data;
    },

    editRentFee: async (id: string, rentFee: number): Promise<Vehicle> => {
      const response = await axios.put<Vehicle>(`${vehiclesURL}/${id}/fee/edit?rentFee=${rentFee}`);
      return response.data;
    },

    removeVehicle: async (id: string): Promise<boolean> => {
      const response = await axios.delete<boolean>(`${vehiclesURL}/${id}/remove`);
      return response.data;
    }
};
