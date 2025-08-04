import logo from "./logo.svg";
import "./App.css";
import Router from "./Routes";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { persistor, store } from './app/store';
import { Provider } from "react-redux";
import { PersistGate } from 'redux-persist/integration/react';
import queryClient from './queryClient';
import AuthInitializer from "./Components/AuthInitializer";
import { useEffect } from "react";
import FriendRequestListener from './Components/Listeners/FriendRequestListner';
import { startSignalRConnection } from './signalr/signalrConnection';

function App() {
  useEffect(() => {
    startSignalRConnection()
  }, [])
  return (
    <div className="App">
      <Provider store={store}>
        <PersistGate loading={null} persistor={persistor}>
          <QueryClientProvider client={queryClient}>
            <FriendRequestListener />
            <AuthInitializer />
            <Router />
          </QueryClientProvider>
        </PersistGate>
      </Provider>
    </div>
  );
}

export default App;
