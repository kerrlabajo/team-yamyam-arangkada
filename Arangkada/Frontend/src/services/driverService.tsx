import axios from 'axios';
import { Driver, PostDriver, PutDriver } from './dataTypes';
import { BASE_URL } from './baseUrl';

const driversURL = BASE_URL + '/drivers';

export const driverService = {
    addDriver: async (driver: PostDriver): Promise<Driver> => {
      const response = await axios.post<Driver>(`${driversURL}/add`, driver);
      return response.data;
    },

    getDriversByOperator: async (operatorId: string): Promise<Driver[]> => {
      const response = await axios.get<Driver[]>(`${driversURL}/by/op/${operatorId}`);
      return response.data;
    },

    getDriverById: async (id: string): Promise<Driver> => {
      const response = await axios.get<Driver>(`${driversURL}/${id}`);
      return response.data;
    },
    
    editDriver: async (id: string, driver: PutDriver): Promise<Driver> => {
      const response = await axios.put<Driver>(`${driversURL}/${id}/edit`, driver);
      return response.data;
    },

    assignDriver: async (id: string, pnum: string): Promise<Driver> => {
      const response = await axios.put<Driver>(`${driversURL}/${id}/assign?pnum=${pnum}`);
      return response.data;
    },    

    removeDriver: async (id: string): Promise<object> => {
      const response = await axios.delete<object>(`${driversURL}/${id}/remove`);
      return response.data;
    }
};
