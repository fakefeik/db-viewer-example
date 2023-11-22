import {DbViewerApplication, DbViewerApi, NullCustomRenderer} from "@skbkontur/db-viewer-ui";
import {BrowserRouter} from "react-router-dom";

function App() {
    return (
        <BrowserRouter>
            <DbViewerApplication
                dbViewerApi={new DbViewerApi("/db-viewer/")}
                customRenderer={new NullCustomRenderer()}
                identifierKeywords={[]}
                useErrorHandlingContainer
                isSuperUser
            />
        </BrowserRouter>
    )
}

export default App
