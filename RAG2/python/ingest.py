import os
import pandas as pd
from langchain_community.document_loaders import UnstructuredFileLoader  # 文件加載器
from langchain_community.embeddings import HuggingFaceEmbeddings  # 嵌入器
from langchain_community.vectorstores import Redis  # 向量儲存器
from langchain_text_splitters import RecursiveCharacterTextSplitter  # 文本分割器
from rag_redis.config import EMBED_MODEL, INDEX_NAME, INDEX_SCHEMA, REDIS_URL  # 配置


def ingest_documents():
    """
    從包含Nike的Edgar 10k文件數據的data/目錄中將Excel文件加載到Redis中。
    """
    try:
        # 加載Excel文件
        data_path = "/RAG2/Views/Pdf/"
        excel_files = [os.path.join(data_path, file) for file in os.listdir(data_path) if file.endswith('.xlsx')]

        for excel_file in excel_files:
            # 讀取Excel文件
            df = pd.read_excel(excel_file, engine='openpyxl')

            # 假設Excel文件中有一個名為'content'的列包含文本數據
            texts = df['suggest'].tolist() + df['sub_suggest'].tolist()

            # 假設文件名就是Excel文件的文件名（不含副檔名）
            file_name = os.path.splitext(os.path.basename(excel_file))[0]

            print("解析Excel文件", excel_file)
            embedder = HuggingFaceEmbeddings(model_name=EMBED_MODEL)

            # 將文本數據嵌入到Redis中
            _ = Redis.from_texts(
                texts=[f"事故分群：{str(file_name)}。" + str(text) for text in texts],
                embedding=embedder,
                index_name=INDEX_NAME,
                index_schema=INDEX_SCHEMA,
                redis_url=REDIS_URL,
            )
    except error as e:
        return e

