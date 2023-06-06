import axios from 'axios';
import { Operator, PostOperator, PutOperator } from './dataTypes';
import { BASE_URL } from './baseUrl';

const operatorsURL = BASE_URL + `/operators`;

export const operatorService = {
    register: async (operator: PostOperator): Promise<Operator> => {
      const response = await axios.post<Operator>(`${operatorsURL}/register`, operator);
      return response.data;
    },

    getOperatorById: async (id: string): Promise<Operator> => {
      const response = await axios.get<Operator>(`${operatorsURL}/${id}`);
      return response.data;
    },

    getOperatorByUserName: async (userName: string): Promise<Operator> => {
      const response = await axios.get<Operator>(`${operatorsURL}/un/${userName}`);
      return response.data;
    },

    getPasswordById: async (id: string): Promise<string> => {
      const response = await axios.get<string>(`${operatorsURL}/${id}/password`);
      return response.data;
    },

    getIsVerifiedById: async (id: string): Promise<boolean> => {
      const response = await axios.get<boolean>(`${operatorsURL}/${id}/status`);
      return response.data;
    },

    editOperator: async (id: string, operator: PutOperator): Promise<Operator> => {
      const response = await axios.put<Operator>(`${operatorsURL}/${id}/edit`, operator);
      return response.data;
    },

    updateVerification: async (id: string, status: boolean): Promise<Operator> => {
        const response = await axios.put<Operator>(`${operatorsURL}/${id}/email?status=${status}`);
      return response.data;
    },

    removeOperator: async (id: string): Promise<object> => {
      const response = await axios.delete<object>(`${operatorsURL}/${id}/remove`);
      return response.data;
    }
};
