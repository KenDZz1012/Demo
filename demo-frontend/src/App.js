import logo from "./logo.svg";
import "./App.css";
import Router from "./Routes";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { persistor, store } from './app/store';
import { Provider } from "react-redux";
import { PersistGate } from 'redux-persist/integration/react';

const queryClient = new QueryClient();

function App() {
  return (
    <div className="App">
      <Provider store={store}>
        <PersistGate loading={null} persistor={persistor}>
          <QueryClientProvider client={queryClient}>
            <Router />
          </QueryClientProvider>
        </PersistGate>
      </Provider>
    </div>
  );
}

export default App;
