import { FetchTestCodes } from "../../../Connections/AppBackend/Catalog/TestCode";

export const GetTestCodes = async () => {
  try {
    const result = await FetchTestCodes();
    console.log(result);
    return result;
  } catch (err) {
    throw err;
  }
};
