import { createContext, useState } from "react";

export type MessageContextType = {
  message: string | null,
  setMessage: (text: string | null) => void,
}

export const MessageContext = createContext<MessageContextType | null>(null);

const MessageContextProvider = (props: { children: React.ReactNode }) => {
  const [message, setMessage] = useState<string | null>(null);

  const value = {
    message,
    setMessage,
  }

  return (
    <MessageContext.Provider value={value}>
      {props.children}
    </MessageContext.Provider>
  )

}

export default MessageContextProvider;
