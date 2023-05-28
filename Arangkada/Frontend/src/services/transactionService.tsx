import axios from 'axios';
import { Transaction, PostTransaction, PutTransaction } from './dataTypes';
import { BASE_URL } from './baseUrl';

const transactionsURL = BASE_URL + '/transactions';

export const transactionService = {
    recordTransaction: async (transaction: PostTransaction): Promise<Transaction> => {
        const response = await axios.post<Transaction>(`${transactionsURL}/record`, transaction);
        return response.data;
    },

    getAllTransactions: async (): Promise<Transaction[]> => {
        const response = await axios.get<Transaction[]>(transactionsURL);
        return response.data;
    },

    getTransactionsByOperator: async (operatorId: string): Promise<Transaction[]> => {
        const response = await axios.get<Transaction[]>(`${transactionsURL}/by/op/${operatorId}`);
        return response.data;
    },

    getTransactionByDriver: async (driverId: string): Promise<Transaction[]> => {
        const response = await axios.get<Transaction[]>(`${transactionsURL}/by/dr/${driverId}`);
        return response.data;
    },

    getTransactionById: async (id: string): Promise<Transaction> => {
        const response = await axios.get<Transaction>(`${transactionsURL}/${id}`);
        return response.data;
    },

    editTransaction: async (id: string, transaction: PutTransaction): Promise<Transaction> => {
        const response = await axios.put<Transaction>(`${transactionsURL}/${id}/edit`, transaction);
        return response.data;
    },

    deleteTransaction: async (id: string): Promise<object> => {
        const response = await axios.delete<object>(`${transactionsURL}/${id}/delete`);
        return response.data;
    }
};